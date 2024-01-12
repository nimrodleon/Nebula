using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Common.Dto;
using Nebula.Modules.Hoteles.Models;
using Nebula.Modules.Hoteles;

namespace Nebula.Controllers.Hoteles;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/hoteles/{companyId}/[controller]")]
[ApiController]
public class CategoriaHabitacionController : ControllerBase
{
    private readonly ICategoriaHabitacionService _categoriaHabitacionService;

    public CategoriaHabitacionController(ICategoriaHabitacionService categoriaHabitacionService)
    {
        _categoriaHabitacionService = categoriaHabitacionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Nombre" };
        var categories = await _categoriaHabitacionService.GetFilteredAsync(companyId, fieldNames, query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var category = await _categoriaHabitacionService.GetByIdAsync(companyId, id);
        return Ok(category);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2(string companyId, [FromQuery] string? term)
    {
        if (string.IsNullOrWhiteSpace(term)) term = string.Empty;
        string[] fieldNames = new string[] { "Nombre" };
        var responseData = await _categoriaHabitacionService.GetFilteredAsync(companyId, fieldNames, term, 10);
        var data = new List<InputSelect2>();
        responseData.ForEach(item =>
        {
            data.Add(new InputSelect2()
            {
                Id = item.Id,
                Text = item.Nombre
            });
        });
        return Ok(new { Results = data });
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] CategoriaHabitacion model)
    {
        model.CompanyId = companyId.Trim();
        model.Nombre = model.Nombre.ToUpper();
        await _categoriaHabitacionService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] CategoriaHabitacion model)
    {
        var pisoHotel = await _categoriaHabitacionService.GetByIdAsync(id);

        model.Id = pisoHotel.Id;
        model.CompanyId = companyId.Trim();
        model.Nombre = model.Nombre.ToUpper();
        model = await _categoriaHabitacionService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = CompanyRoles.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var categoriaHabitacion = await _categoriaHabitacionService.GetByIdAsync(companyId, id);
        await _categoriaHabitacionService.RemoveAsync(companyId, categoriaHabitacion.Id);
        return Ok(categoriaHabitacion);
    }
}
