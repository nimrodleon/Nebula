using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Models;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.InvoiceHub.Dto;
using Nebula.Modules.InvoiceHub.Helpers;
using System.Security.Cryptography.X509Certificates;

namespace Nebula.Controllers.Account;

[Authorize]
[Route("api/account/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly ICollaboratorService _collaboratorService;
    private readonly ICacheAuthService _cacheAuthService;
    private readonly ICertificadoUploaderService _certificadoUploaderService;

    public CompanyController(ICompanyService companyService,
        ICollaboratorService collaboratorService, ICacheAuthService cacheAuthService,
        ICertificadoUploaderService certificadoUploaderService)
    {
        _companyService = companyService;
        _collaboratorService = collaboratorService;
        _cacheAuthService = cacheAuthService;
        _certificadoUploaderService = certificadoUploaderService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var companies = await _companyService.GetAsync("RznSocial", query);
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var company = await _companyService.GetByIdAsync(id);
        return Ok(company);
    }

    [HttpGet("Info/{id}")]
    public async Task<IActionResult> GetCompanyInfo(string id)
    {
        var company = await _companyService.GetByIdAsync(id);
        if (company == null) return NotFound();
        return Ok(new
        {
            company.Id,
            company.Ruc,
            company.RznSocial,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Company model)
    {
        model.Ruc = model.Ruc.Trim();
        model.RznSocial = model.RznSocial.Trim().ToUpper();
        model.Address = model.Address.Trim().ToUpper();
        model.Departamento = model.Departamento.Trim().ToUpper();
        model.Provincia = model.Provincia.Trim().ToUpper();
        model.Distrito = model.Distrito.Trim().ToUpper();
        model.Urbanizacion = model.Urbanizacion.Trim().ToUpper();
        model = await _companyService.CreateAsync(model);

        // agregar empresa en cache.
        var companies = await _cacheAuthService.GetUserAuthCompaniesAsync(model.UserId);
        if (companies == null)
        {
            await _cacheAuthService.SetUserAuthCompaniesAsync(model.UserId, new List<Company> { model });
        }
        else
        {
            companies.Add(model);
            await _cacheAuthService.RemoveUserAuthCompaniesAsync(model.UserId);
            await _cacheAuthService.SetUserAuthCompaniesAsync(model.UserId, companies);
        }

        var collaborator = new Collaborator()
        {
            CompanyId = model.Id,
            UserId = model.UserId,
            UserRole = CompanyRoles.Owner,
        };
        await _collaboratorService.CreateAsync(collaborator);

        // agregar rol a cache.
        var userCompanyRole = new UserCompanyRole()
        {
            CompanyId = collaborator.CompanyId,
            UserRole = collaborator.UserRole,
        };
        var userCompanyRoles = await _cacheAuthService.GetUserAuthCompanyRolesAsync(model.UserId);
        if (userCompanyRoles == null)
        {
            await _cacheAuthService.SetUserAuthCompanyRolesAsync(model.UserId, new List<UserCompanyRole> { userCompanyRole });
        }
        else
        {
            userCompanyRoles.Add(userCompanyRole);
            await _cacheAuthService.RemoveUserAuthCompanyRolesAsync(model.UserId);
            await _cacheAuthService.SetUserAuthCompanyRolesAsync(model.UserId, userCompanyRoles);
        }

        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Company model)
    {
        var company = await _companyService.GetByIdAsync(id);
        model.Id = company.Id;
        model.Ruc = model.Ruc.Trim();
        model.RznSocial = model.RznSocial.Trim().ToUpper();
        model.Address = model.Address.Trim().ToUpper();
        model.Departamento = model.Departamento.Trim().ToUpper();
        model.Provincia = model.Provincia.Trim().ToUpper();
        model.Distrito = model.Distrito.Trim().ToUpper();
        model.Urbanizacion = model.Urbanizacion.Trim().ToUpper();
        company = await _companyService.UpdateAsync(company.Id, model);
        // actualizar empresa en cache.
        var companies = await _cacheAuthService.GetUserAuthCompaniesAsync(model.UserId);
        if (companies != null)
        {
            var index = companies.FindIndex(x => x.Id == company.Id);
            if (index != -1)
            {
                companies[index] = company;
                await _cacheAuthService.RemoveUserAuthCompaniesAsync(model.UserId);
                await _cacheAuthService.SetUserAuthCompaniesAsync(company.UserId, companies);
            }
        }
        return Ok(company);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var company = await _companyService.GetByIdAsync(id);
        await _companyService.RemoveAsync(id);
        return Ok(company);
    }

    [HttpPost("SubirCertificado/{companyId}")]
    public async Task<IActionResult> SubirCertificado(string companyId, [FromForm] IFormFile certificate, [FromForm] string password)
    {
        try
        {
            var company = await _companyService.GetByIdAsync(companyId.Trim());
            if (company == null) return BadRequest(new { ok = false, msg = "No existe la Empresa." });

            // Leer el archivo de certificado como un array de bytes
            using (var ms = new MemoryStream())
            {
                await certificate.CopyToAsync(ms);
                byte[] certificadoPfx = ms.ToArray();
                var result = await _certificadoUploaderService.SubirCertificado(certificadoPfx, password, company.Ruc, company.Id);
                // actualizar fecha de vencimiento.
                company.FechaVencimientoCert = new X509Certificate2(certificadoPfx, password.Trim()).NotAfter.ToString("yyyy-MM-dd");
                company.SunatEndpoint = SunatEndpoints.FeBeta;
                await _companyService.UpdateAsync(company.Id, company);
                // sincronizar datos de la empresa...
                return Ok(company);
            }
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que ocurra durante el procesamiento del certificado
            return BadRequest(new { ok = false, msg = $"Error al procesar el certificado: {ex.Message}" });
        }
    }
}
