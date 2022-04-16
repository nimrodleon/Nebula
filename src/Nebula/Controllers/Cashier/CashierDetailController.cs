using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Helpers;
using Nebula.Data.Models.Cashier;
using Nebula.Data.Services.Cashier;

namespace Nebula.Controllers.Cashier;

[Authorize(Roles = AuthRoles.User)]
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
        if (model.TypeOperation == TypeOperationCaja.EntradaDeDinero)
            model.TypeOperation = TypeOperationCaja.EntradaDeDinero;
        if (model.TypeOperation == TypeOperationCaja.SalidaDeDinero)
            model.TypeOperation = TypeOperationCaja.SalidaDeDinero;
        model.FormaPago = FormaPago.Contado;
        await _cashierDetailService.CreateAsync(model);
        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = "La operación ha sido registrado!"
        });
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var cashierDetail = await _cashierDetailService.GetAsync(id);
        await _cashierDetailService.RemoveAsync(cashierDetail.Id);
        return Ok(new {Ok = true, Data = cashierDetail, Msg = "El detalle de caja ha sido borrado!"});
    }
}
