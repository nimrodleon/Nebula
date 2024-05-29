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
using Nebula.Modules.Contacts;
using Nebula.Common.Helpers;

namespace Nebula.Controllers.Cashier;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/cashier/[controller]")]
[ApiController]
public class CajaDiariaController(
    IUserAuthenticationService userAuthenticationService,
    ICompanyService companyService,
    ICajaDiariaService cajaDiariaService,
    IInvoiceSerieService invoiceSerieService,
    ICashierDetailService cashierDetailService,
    IContactService contactService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] DateQuery model, [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var cajasDiarias = await cajaDiariaService.GetCajasDiariasAsync(_companyId, model, page, pageSize);
        var totalCajasDiarias = await cajaDiariaService.GetTotalCajasDiariasAsync(_companyId, model);
        var totalPages = (int)Math.Ceiling((double)totalCajasDiarias / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<CajaDiaria>
        {
            Pagination = paginationInfo,
            Data = cajasDiarias
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var cajaDiaria = await cajaDiariaService.GetByIdAsync(_companyId, id);
        var invoiceSerie = await invoiceSerieService.GetByIdAsync(_companyId, cajaDiaria.InvoiceSerieId);
        cajaDiaria.WarehouseId = invoiceSerie.WarehouseId;
        return Ok(cajaDiaria);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AperturaCaja model)
    {
        var invoiceSerie = await invoiceSerieService.GetByIdAsync(_companyId, model.InvoiceSerie);
        var cajaDiaria = new CajaDiaria()
        {
            CompanyId = _companyId.Trim(),
            InvoiceSerieId = invoiceSerie.Id,
            Terminal = invoiceSerie.Name,
            Status = "ABIERTO",
            TotalApertura = model.Total,
            TotalCierre = 0.0M,
            Turno = model.Turno
        };
        await cajaDiariaService.InsertOneAsync(cajaDiaria);

        // registrar apertura de caja.
        var detalleCaja = new CashierDetail()
        {
            CompanyId = _companyId.Trim(),
            CajaDiariaId = cajaDiaria.Id,
            Remark = "APERTURA DE CAJA",
            TypeOperation = TipoOperationCaja.AperturaDeCaja,
            FormaPago = MetodosPago.Contado,
            Amount = model.Total
        };
        await cashierDetailService.InsertOneAsync(detalleCaja);

        return Ok(cajaDiaria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CerrarCaja model)
    {
        var cajaDiaria = await cajaDiariaService.GetByIdAsync(_companyId, id);
        cajaDiaria.CompanyId = _companyId.Trim();
        cajaDiaria.TotalCierre = model.TotalCierre;
        cajaDiaria.Status = "CERRADO";
        await cajaDiariaService.ReplaceOneAsync(id, cajaDiaria);
        return Ok(cajaDiaria);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var cajaDiaria = await cajaDiariaService.GetByIdAsync(_companyId, id);
        await cajaDiariaService.DeleteOneAsync(_companyId, cajaDiaria.Id);
        return Ok(cajaDiaria);
    }

    [HttpGet("CajasAbiertas")]
    public async Task<IActionResult> CajasAbiertas()
    {
        return Ok(await cajaDiariaService.GetCajasAbiertasAsync(_companyId));
    }

    /// <summary>
    /// Configuración Inicial para el punto de venta.
    /// </summary>
    /// <param name="companyId">Identificador de la empresa</param>
    /// <param name="id">Identificador de la caja diaria</param>
    /// <returns>Configuración</returns>
    [HttpGet("GetQuickSaleConfig/{id}")]
    public async Task<IActionResult> GetQuickSaleConfig(string id)
    {
        var company = await companyService.GetByIdAsync(_companyId.Trim());
        var cajaDiaria = await cajaDiariaService.GetByIdAsync(_companyId, id);
        var invoiceSerie = await invoiceSerieService.GetByIdAsync(_companyId, cajaDiaria.InvoiceSerieId);
        cajaDiaria.WarehouseId = invoiceSerie.WarehouseId;
        var contact = await contactService.GetByIdAsync(_companyId, company.ContactId);

        var quickSaleConfig = new QuickSaleConfig()
        {
            Company = company,
            CajaDiaria = cajaDiaria,
            Contact = contact
        };
        return Ok(quickSaleConfig);
    }
}
