using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Models.Cashier;
using Nebula.Data.Services.Cashier;

namespace Nebula.Controllers.Cashier;

[Route("api/[controller]")]
[ApiController]
public class CashierDetailController : ControllerBase
{
    private readonly CashierDetailService _cashierDetailService;

    public CashierDetailController(CashierDetailService cashierDetailService) =>
        _cashierDetailService = cashierDetailService;

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(string id, [FromQuery] string? query)
    {
        var responseData = await _cashierDetailService.GetListAsync(id, query);
        return Ok(responseData);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CashierDetail model)
    {
        model.TypeOperation = TypeOperation.CajaChica;
        model.FormaPago = "Contado";
        await _cashierDetailService.CreateAsync(model);
        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = "La operación ha sido registrado!"
        });
    }
}
