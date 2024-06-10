using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common.Helpers;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.InvoiceHub.Dto;
using Nebula.Modules.InvoiceHub.Helpers;
using System.Security.Cryptography.X509Certificates;

namespace Nebula.Controllers.Account;

[Authorize]
[Route("api/account/[controller]")]
[ApiController]
public class CompanyController(
    ICompanyService companyService,
    ICertificadoUploaderService certificadoUploaderService,
    IEmpresaHubService empresaHubService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var companies = await companyService.GetAsync("RznSocial", query);
        return Ok(companies);
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var companies = await companyService.GetCompaniesAsync(query, page, pageSize);
        var totalCompanies = await companyService.GetTotalCompaniesAsync(query);
        var totalPages = (int)Math.Ceiling((double)totalCompanies / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<Company>
        {
            Pagination = paginationInfo,
            Data = companies
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var company = await companyService.GetByIdAsync(id);
        return Ok(company);
    }

    [HttpGet("Info/{id}")]
    public async Task<IActionResult> GetCompanyInfo(string id)
    {
        var company = await companyService.GetByIdAsync(id);
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
        model = await companyService.InsertOneAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Company model)
    {
        var company = await companyService.GetByIdAsync(id);
        model.Id = company.Id;
        model.Ruc = model.Ruc.Trim();
        model.RznSocial = model.RznSocial.Trim().ToUpper();
        model.Address = model.Address.Trim().ToUpper();
        model.Departamento = model.Departamento.Trim().ToUpper();
        model.Provincia = model.Provincia.Trim().ToUpper();
        model.Distrito = model.Distrito.Trim().ToUpper();
        model.Urbanizacion = model.Urbanizacion.Trim().ToUpper();
        company = await companyService.ReplaceOneAsync(company.Id, model);
        return Ok(company);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var company = await companyService.GetByIdAsync(id);
        await companyService.DeleteOneAsync(id);
        return Ok(company);
    }

    [HttpPost("SubirCertificado/{companyId}")]
    public async Task<IActionResult> SubirCertificado(string companyId, [FromForm] string password,
        [FromForm] string extension, IFormFile certificate)
    {
        try
        {
            var company = await companyService.GetByIdAsync(companyId.Trim());
            if (company == null) return BadRequest(new { ok = false, msg = "No existe la Empresa." });

            // Leer el archivo de certificado como un array de bytes
            using (var ms = new MemoryStream())
            {
                await certificate.CopyToAsync(ms);
                byte[] certificado = ms.ToArray();
                var result =
                    await certificadoUploaderService.SubirCertificado(certificado, password, company.Id, extension);
                // actualizar fecha de vencimiento.
                company.FechaVencimientoCert =
                    new X509Certificate2(certificado, password.Trim()).NotAfter.ToString("yyyy-MM-dd");
                company.SunatEndpoint = SunatEndpoints.FeBeta;
                await companyService.ReplaceOneAsync(company.Id, company);
                // sincronizar datos de la empresa.
                var empresaHub = new EmpresaHub()
                {
                    Ruc = company.Ruc.Trim(),
                    CompanyId = company.Id.Trim(),
                    RazonSocial = company.RznSocial.Trim(),
                    NombreComercial = company.RznSocial.Trim(),
                    SunatEndpoint = company.SunatEndpoint.Trim(),
                    ClaveSol = new ClaveSolHub()
                    {
                        User = company.ClaveSol.User.Trim(),
                        Password = company.ClaveSol.Password.Trim(),
                    },
                    Address = new AddressHub()
                    {
                        Ubigueo = company.Ubigueo.Trim(),
                        Departamento = company.Departamento.Trim(),
                        Provincia = company.Provincia.Trim(),
                        Distrito = company.Distrito.Trim(),
                        Urbanizacion = company.Urbanizacion.Trim(),
                        Direccion = company.Address.Trim(),
                        CodLocal = company.CodLocalEmisor.Trim(),
                    }
                };
                await empresaHubService.RegistrarEmpresa(empresaHub);

                return Ok(company);
            }
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que ocurra durante el procesamiento del certificado
            return BadRequest(new { ok = false, msg = $"Error al procesar el certificado: {ex.Message}" });
        }
    }

    [HttpPatch("SincronizarDatos/{companyId}")]
    public async Task<IActionResult> SincronizarDatos(string companyId)
    {
        var company = await companyService.GetByIdAsync(companyId.Trim());
        if (company == null) return BadRequest(new { ok = false, msg = "No existe la Empresa." });
        // sincronizar datos de la empresa.
        var empresaHub = new EmpresaHub()
        {
            Ruc = company.Ruc.Trim(),
            CompanyId = company.Id.Trim(),
            RazonSocial = company.RznSocial.Trim(),
            NombreComercial = company.RznSocial.Trim(),
            SunatEndpoint = company.SunatEndpoint.Trim(),
            ClaveSol = new ClaveSolHub()
            {
                User = company.ClaveSol.User.Trim(),
                Password = company.ClaveSol.Password.Trim(),
            },
            Address = new AddressHub()
            {
                Ubigueo = company.Ubigueo.Trim(),
                Departamento = company.Departamento.Trim(),
                Provincia = company.Provincia.Trim(),
                Distrito = company.Distrito.Trim(),
                Urbanizacion = company.Urbanizacion.Trim(),
                Direccion = company.Address.Trim(),
                CodLocal = company.CodLocalEmisor.Trim(),
            }
        };
        await empresaHubService.RegistrarEmpresa(empresaHub);
        return Ok(company);
    }

    [HttpPatch("QuitarCertificado/{companyId}")]
    public async Task<IActionResult> QuitarCertificado(string companyId)
    {
        var company = await companyService.GetByIdAsync(companyId.Trim());
        if (company == null) return BadRequest(new { ok = false, msg = "No existe la Empresa." });
        company.FechaVencimientoCert = "-";
        company.SunatEndpoint = SunatEndpoints.FeBeta;
        await companyService.ReplaceOneAsync(company.Id, company);
        return Ok(company);
    }

    [HttpPatch("CambiarSunatEndpoint/{companyId}")]
    public async Task<IActionResult> CambiarSunatEndpoint(string companyId)
    {
        var company = await companyService.GetByIdAsync(companyId.Trim());
        if (company == null) return BadRequest(new { ok = false, msg = "No existe la Empresa." });
        if (company.SunatEndpoint == SunatEndpoints.FeBeta)
            company.SunatEndpoint = SunatEndpoints.FeProduccion;
        else if (company.SunatEndpoint == SunatEndpoints.FeProduccion)
            company.SunatEndpoint = SunatEndpoints.FeBeta;
        await companyService.ReplaceOneAsync(company.Id, company);
        // sincronizar datos de la empresa.
        var empresaHub = new EmpresaHub()
        {
            Ruc = company.Ruc.Trim(),
            CompanyId = company.Id.Trim(),
            RazonSocial = company.RznSocial.Trim(),
            NombreComercial = company.RznSocial.Trim(),
            SunatEndpoint = company.SunatEndpoint.Trim(),
            ClaveSol = new ClaveSolHub()
            {
                User = company.ClaveSol.User.Trim(),
                Password = company.ClaveSol.Password.Trim(),
            },
            Address = new AddressHub()
            {
                Ubigueo = company.Ubigueo.Trim(),
                Departamento = company.Departamento.Trim(),
                Provincia = company.Provincia.Trim(),
                Distrito = company.Distrito.Trim(),
                Urbanizacion = company.Urbanizacion.Trim(),
                Direccion = company.Address.Trim(),
                CodLocal = company.CodLocalEmisor.Trim(),
            }
        };
        await empresaHubService.RegistrarEmpresa(empresaHub);
        return Ok(company);
    }
}
