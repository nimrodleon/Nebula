using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Products.Models;
using Nebula.Common.Dto;
using Nebula.Modules.Products;
using Microsoft.AspNetCore.Authorization;

namespace Nebula.Controllers.Products;

[Authorize]
[Route("api/products/{companyId}/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService) =>
        _categoryService = categoryService;

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Name" };
        var categories = await _categoryService.GetFilteredAsync(companyId, fieldNames, query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var category = await _categoryService.GetByIdAsync(companyId, id);
        return Ok(category);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2(string companyId, [FromQuery] string? term)
    {
        if (string.IsNullOrWhiteSpace(term)) term = string.Empty;
        string[] fieldNames = new string[] { "Name" };
        var responseData = await _categoryService.GetFilteredAsync(companyId, fieldNames, term, 10);
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
    public async Task<IActionResult> Create(string companyId, [FromBody] Category model)
    {
        model.CompanyId = companyId.Trim();
        model.Name = model.Name.ToUpper();
        await _categoryService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Category model)
    {
        var category = await _categoryService.GetByIdAsync(id);

        model.Id = category.Id;
        model.CompanyId = companyId.Trim();
        model.Name = model.Name.ToUpper();
        model = await _categoryService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var category = await _categoryService.GetByIdAsync(companyId, id);
        await _categoryService.RemoveAsync(companyId, category.Id);
        return Ok(category);
    }
}
