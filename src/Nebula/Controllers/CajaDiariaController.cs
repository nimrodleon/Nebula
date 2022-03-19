using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Models;
using Nebula.Data.Services;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CajaDiariaController : ControllerBase
{
    private readonly CajaDiariaService _cajaDiariaService;
    private readonly InvoiceSerieService _invoiceSerieService;
    private readonly CashierDetailService _cashierDetailService;

    public CajaDiariaController(CajaDiariaService cajaDiariaService,
        InvoiceSerieService invoiceSerieService, CashierDetailService cashierDetailService)
    {
        _cajaDiariaService = cajaDiariaService;
        _invoiceSerieService = invoiceSerieService;
        _cashierDetailService = cashierDetailService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var cajaDiarias = await _cajaDiariaService.GetListAsync(model);
        return Ok(cajaDiarias);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var cajaDiaria = await _cajaDiariaService.GetAsync(id);
        return Ok(cajaDiaria);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] AperturaCaja model)
    {
        var invoiceSerie = await _invoiceSerieService.GetAsync(model.SerieId);
        var cajaDiaria = new CajaDiaria()
        {
            Terminal = $"{invoiceSerie.Id}:{invoiceSerie.Name}",
            Status = "ABIERTO",
            TotalApertura = model.Total,
            TotalContabilizado = 0.0M,
            TotalCierre = 0.0M,
            Turno = model.Turno
        };
        await _cajaDiariaService.CreateAsync(cajaDiaria);

        // registrar apertura de caja.
        var detalleCaja = new CashierDetail()
        {
            CajaDiaria = cajaDiaria.Id,
            Remark = "APERTURA DE CAJA",
            Type = "ENTRADA",
            TypeOperation = TypeOperation.CajaChica,
            FormaPago = "Contado",
            Amount = model.Total
        };
        await _cashierDetailService.CreateAsync(detalleCaja);

        return Ok(new
        {
            Ok = true,
            Data = cajaDiaria,
            Msg = "La apertura de caja ha sido registrado!"
        });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CerrarCaja model)
    {
        var cajaDiaria = await _cajaDiariaService.GetAsync(id);
        cajaDiaria.TotalContabilizado = model.TotalContabilizado;
        cajaDiaria.TotalCierre = model.TotalCierre;
        cajaDiaria.Status = "CERRADO";
        await _cajaDiariaService.UpdateAsync(id, cajaDiaria);
        return Ok(new
        {
            Ok = true,
            Data = cajaDiaria,
            Msg = "El cierre de caja ha sido registrado!"
        });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var cajaDiaria = await _cajaDiariaService.GetAsync(id);
        await _cajaDiariaService.RemoveAsync(cajaDiaria.Id);
        return Ok(new {Ok = true, Data = cajaDiaria, Msg = "La caja diaria ha sido borrado!"});
    }
}
