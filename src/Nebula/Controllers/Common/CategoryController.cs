using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Models.Common;
using Nebula.Data.Services;
using Nebula.Data.Services.Common;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService) =>
        _categoryService = categoryService;

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _categoryService.GetListAsync(query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var category = await _categoryService.GetAsync(id);
        return Ok(category);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2([FromQuery] string? term)
    {
        if (string.IsNullOrWhiteSpace(term)) term = string.Empty;
        var responseData = await _categoryService.GetListAsync(term, 10);
        var data = new List<Select2>();
        responseData.ForEach(item =>
        {
            data.Add(new Select2()
            {
                Id = item.Id,
                Text = item.Name
            });
        });
        return Ok(new { Results = data });
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Category model)
    {
        model.Name = model.Name.ToUpper();
        await _categoryService.CreateAsync(model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"La Categoría {model.Name} ha sido registrado!"
        });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Category model)
    {
        var category = await _categoryService.GetAsync(id);

        model.Id = category.Id;
        model.Name = model.Name.ToUpper();
        await _categoryService.UpdateAsync(id, model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"La Categoría {model.Name} ha sido actualizado!"
        });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var category = await _categoryService.GetAsync(id);
        await _categoryService.RemoveAsync(id);
        return Ok(new { Ok = true, Data = category, Msg = "La categoría ha sido borrado!" });
    }
}
