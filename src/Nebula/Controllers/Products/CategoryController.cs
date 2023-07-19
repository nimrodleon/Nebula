using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Products.Models;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Dto;
using Nebula.Modules.Products;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService) =>
        _categoryService = categoryService;

    [HttpGet("Index"), UserAuthorize(Permission.ProductRead)]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _categoryService.GetAsync("Name", query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.ProductRead)]
    public async Task<IActionResult> Show(string id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return Ok(category);
    }

    [HttpGet("Select2"), UserAuthorize(Permission.ProductRead)]
    public async Task<IActionResult> Select2([FromQuery] string? term)
    {
        if (string.IsNullOrWhiteSpace(term)) term = string.Empty;
        var responseData = await _categoryService.GetAsync("Name", term, 10);
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

    [HttpPost("Create"), UserAuthorize(Permission.ProductCreate)]
    public async Task<IActionResult> Create([FromBody] Category model)
    {
        model.Name = model.Name.ToUpper();
        await _categoryService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.ProductEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] Category model)
    {
        var category = await _categoryService.GetByIdAsync(id);

        model.Id = category.Id;
        model.Name = model.Name.ToUpper();
        model = await _categoryService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.ProductDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        await _categoryService.RemoveAsync(id);
        return Ok(category);
    }
}
