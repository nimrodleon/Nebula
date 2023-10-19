using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Models;

namespace Nebula.Controllers.Account
{
    [Authorize]
    [Route("api/account/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ICollaboratorService _collaboratorService;
        private readonly ICacheAuthService _cacheService;

        public CompanyController(ICompanyService companyService,
            ICollaboratorService collaboratorService, ICacheAuthService cacheService)
        {
            _companyService = companyService;
            _collaboratorService = collaboratorService;
            _cacheService = cacheService;
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
            model = await _companyService.CreateAsync(model);
            var companies = await _companyService.GetCompaniesByUserIdAsync(model.UserId);
            await _cacheService.SetUserAuthCompaniesAsync(model.UserId, companies);
            var collaborator = new Collaborator()
            {
                CompanyId = model.Id,
                UserId = model.UserId,
                UserRole = CompanyRoles.Owner,
                IsEmailVerified = true,
            };
            await _collaboratorService.CreateAsync(collaborator);
            var userCompanyRole = new UserCompanyRole()
            {
                CompanyId = collaborator.CompanyId,
                UserRole = collaborator.UserRole,
            };
            await _cacheService.SetUserAuthCompanyRolesAsync(model.UserId, new List<UserCompanyRole> { userCompanyRole });
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
            company = await _companyService.UpdateAsync(company.Id, model);
            var companies = await _companyService.GetCompaniesByUserIdAsync(company.UserId);
            await _cacheService.SetUserAuthCompaniesAsync(company.UserId, companies);
            return Ok(company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var company = await _companyService.GetByIdAsync(id);
            await _companyService.RemoveAsync(id);
            return Ok(company);
        }

    }
}
