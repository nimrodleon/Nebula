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
using Nebula.Common;
using Nebula.Modules.Contacts;

namespace Nebula.Controllers.Cashier;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/cashier/{companyId}/[controller]")]
[ApiController]
public class CajaDiariaController : ControllerBase
{
    private readonly ICacheAuthService _cacheAuthService;
    private readonly ICajaDiariaService _cajaDiariaService;
    private readonly IInvoiceSerieService _invoiceSerieService;
    private readonly ICashierDetailService _cashierDetailService;
    private readonly IContactService _contactService;

    public CajaDiariaController(
        ICacheAuthService cacheAuthService,
        ICajaDiariaService cajaDiariaService,
        IInvoiceSerieService invoiceSerieService,
        ICashierDetailService cashierDetailService,
        IContactService contactService)
    {
        _cacheAuthService = cacheAuthService;
        _cajaDiariaService = cajaDiariaService;
        _invoiceSerieService = invoiceSerieService;
        _cashierDetailService = cashierDetailService;
        _contactService = contactService;
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
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, cajaDiaria.InvoiceSerieId);
        cajaDiaria.WarehouseId = invoiceSerie.WarehouseId;
        return Ok(cajaDiaria);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] AperturaCaja model)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, model.InvoiceSerie);
        var cajaDiaria = new CajaDiaria()
        {
            CompanyId = companyId.Trim(),
            InvoiceSerieId = invoiceSerie.Id,
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

    /// <summary>
    /// Configuración Inicial para el punto de venta.
    /// </summary>
    /// <param name="companyId">Identificador de la empresa</param>
    /// <param name="id">Identificador de la caja diaria</param>
    /// <returns>Configuración</returns>
    [HttpGet("GetQuickSaleConfig/{id}")]
    public async Task<IActionResult> GetQuickSaleConfig(string companyId, string id)
    {
        var company = await _cacheAuthService.GetCompanyByIdAsync(companyId);
        var cajaDiaria = await _cajaDiariaService.GetByIdAsync(companyId, id);
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, cajaDiaria.InvoiceSerieId);
        cajaDiaria.WarehouseId = invoiceSerie.WarehouseId;
        var contact = await _contactService.GetByIdAsync(companyId, company.ContactId);

        var quickSaleConfig = new QuickSaleConfig()
        {
            Company = company,
            CajaDiaria = cajaDiaria,
            Contact = contact
        };
        return Ok(quickSaleConfig);
    }
}
