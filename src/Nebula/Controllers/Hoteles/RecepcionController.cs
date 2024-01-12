using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common.Helpers;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Hoteles;
using Nebula.Modules.Hoteles.Dto;

namespace Nebula.Controllers.Hoteles;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/hoteles/{companyId}/[controller]")]
[ApiController]
public class RecepcionController : ControllerBase
{
    private readonly IHabitacionHotelService _habitacionHotelService;

    public RecepcionController(IHabitacionHotelService habitacionHotelService)
    {
        _habitacionHotelService = habitacionHotelService;
    }

    [HttpGet("HabitacionesDisponibles")]
    public async Task<IActionResult> HabitacionesDisponibles(string companyId, [FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var habitacionesDisponibles = await _habitacionHotelService.GetHabitacionesDisponiblesAsync(companyId, query, page, pageSize);
        var totalHabitacionesDisponibles = await _habitacionHotelService.GetTotalHabitacionesDisponiblesAsync(companyId, query);
        var totalPages = (int)Math.Ceiling((double)totalHabitacionesDisponibles / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<HabitacionDisponible>
        {
            Pagination = paginationInfo,
            Data = habitacionesDisponibles
        };

        return Ok(result);
    }
}
