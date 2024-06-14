using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Products.Models;
using Nebula.Common.Dto;
using Nebula.Modules.Products;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Products;

[Authorize]
[PersonalAuthorize(UserRole = UserRole.User)]
[Route("api/products/[controller]")]
[ApiController]
public class CategoryController(
    IUserAuthenticationService userAuthenticationService,
    ICategoryService categoryService,
    ILogger<CategoryController> logger)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string query = "")
    {
        logger.Log(LogLevel.Information, "Get all categories");
        string[] fieldNames = new string[] { "Name" };
        var categories = await categoryService.GetFilteredAsync(_companyId, fieldNames, query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var category = await categoryService.GetByIdAsync(_companyId, id);
        return Ok(category);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2([FromQuery] string? term)
    {
        if (string.IsNullOrWhiteSpace(term)) term = string.Empty;
        string[] fieldNames = new string[] { "Name" };
        var responseData = await categoryService.GetFilteredAsync(_companyId, fieldNames, term, 10);
        var data = new List<InputSelect2>();
        responseData.ForEach(item =>
        {
            data.Add(new InputSelect2()
            {
                Id = item.Id,
                Text = item.Name
            });
        });
        return Ok(new { Results = data });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category model)
    {
        model.CompanyId = _companyId.Trim();
        model.Name = model.Name.ToUpper();
        await categoryService.InsertOneAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Category model)
    {
        var category = await categoryService.GetByIdAsync(id);

        model.Id = category.Id;
        model.CompanyId = _companyId.Trim();
        model.Name = model.Name.ToUpper();
        model = await categoryService.ReplaceOneAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}"), PersonalAuthorize(UserRole = UserRole.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var category = await categoryService.GetByIdAsync(_companyId, id);
        await categoryService.DeleteOneAsync(_companyId, category.Id);
        return Ok(category);
    }
}
