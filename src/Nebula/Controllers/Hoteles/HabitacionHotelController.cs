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
public class HabitacionHotelController : ControllerBase
{
    private readonly IHabitacionHotelService _habitacionHotelService;

    public HabitacionHotelController(IHabitacionHotelService habitacionHotelService)
    {
        _habitacionHotelService = habitacionHotelService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Nombre" };
        var habitacionesHotel = await _habitacionHotelService.GetFilteredAsync(companyId, fieldNames, query);
        return Ok(habitacionesHotel);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var habitacionHotel = await _habitacionHotelService.GetByIdAsync(companyId, id);
        return Ok(habitacionHotel);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2(string companyId, [FromQuery] string? term)
    {
        if (string.IsNullOrWhiteSpace(term)) term = string.Empty;
        string[] fieldNames = new string[] { "Nombre" };
        var responseData = await _habitacionHotelService.GetFilteredAsync(companyId, fieldNames, term, 10);
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
    public async Task<IActionResult> Create(string companyId, [FromBody] HabitacionHotel model)
    {
        model.CompanyId = companyId.Trim();
        model.Nombre = model.Nombre.ToUpper();
        await _habitacionHotelService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] HabitacionHotel model)
    {
        var habitacionHotel = await _habitacionHotelService.GetByIdAsync(id);

        model.Id = habitacionHotel.Id;
        model.CompanyId = companyId.Trim();
        model.Nombre = model.Nombre.ToUpper();
        model = await _habitacionHotelService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = CompanyRoles.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var habitacionHotel = await _habitacionHotelService.GetByIdAsync(companyId, id);
        await _habitacionHotelService.RemoveAsync(companyId, habitacionHotel.Id);
        return Ok(habitacionHotel);
    }
}
