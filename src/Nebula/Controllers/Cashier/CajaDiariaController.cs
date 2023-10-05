using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Cashier;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Cashier.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Common.Dto;
using Nebula.Modules.Account;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Cashier;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/cashier/{companyId}/[controller]")]
[ApiController]
public class CajaDiariaController : ControllerBase
{
    private readonly ICajaDiariaService _cajaDiariaService;
    private readonly IInvoiceSerieService _invoiceSerieService;
    private readonly ICashierDetailService _cashierDetailService;

    public CajaDiariaController(
        ICajaDiariaService cajaDiariaService,
        IInvoiceSerieService invoiceSerieService,
        ICashierDetailService cashierDetailService)
    {
        _cajaDiariaService = cajaDiariaService;
        _invoiceSerieService = invoiceSerieService;
        _cashierDetailService = cashierDetailService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var cajaDiarias = await _cajaDiariaService.GetListAsync(companyId, model);
        return Ok(cajaDiarias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var cajaDiaria = await _cajaDiariaService.GetByIdAsync(companyId, id);
        return Ok(cajaDiaria);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] AperturaCaja model)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, model.InvoiceSerie);
        var cajaDiaria = new CajaDiaria()
        {
            CompanyId = companyId.Trim(),
            InvoiceSerie = invoiceSerie.Id,
            Terminal = invoiceSerie.Name,
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
            CompanyId = companyId.Trim(),
            CajaDiariaId = cajaDiaria.Id,
            Remark = "APERTURA DE CAJA",
            TypeOperation = TypeOperationCaja.AperturaDeCaja,
            FormaPago = FormaPago.Contado,
            Amount = model.Total
        };
        await _cashierDetailService.CreateAsync(detalleCaja);

        return Ok(cajaDiaria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] CerrarCaja model)
    {
        var cajaDiaria = await _cajaDiariaService.GetByIdAsync(companyId, id);
        cajaDiaria.CompanyId = companyId.Trim();
        cajaDiaria.TotalContabilizado = model.TotalContabilizado;
        cajaDiaria.TotalCierre = model.TotalCierre;
        cajaDiaria.Status = "CERRADO";
        await _cajaDiariaService.UpdateAsync(id, cajaDiaria);
        return Ok(cajaDiaria);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var cajaDiaria = await _cajaDiariaService.GetByIdAsync(companyId, id);
        await _cajaDiariaService.RemoveAsync(companyId, cajaDiaria.Id);
        return Ok(cajaDiaria);
    }

    [HttpGet("CajasAbiertas")]
    public async Task<IActionResult> CajasAbiertas(string companyId)
    {
        return Ok(await _cajaDiariaService.GetCajasAbiertasAsync(companyId));
    }
}
