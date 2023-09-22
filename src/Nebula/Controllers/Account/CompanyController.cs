using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;

namespace Nebula.Controllers.Account
{
    [Authorize]
    [Route("api/account/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Company model)
        {
            model.Ruc = model.Ruc.Trim();
            model.RznSocial = model.RznSocial.Trim().ToUpper();
            model.Address = model.Address.Trim().ToUpper();
            await _companyService.CreateAsync(model);
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
