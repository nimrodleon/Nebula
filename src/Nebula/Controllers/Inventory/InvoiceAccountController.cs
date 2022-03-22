using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class InvoiceAccountController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ApplicationDbContext _context;

    public InvoiceAccountController(ILogger<InvoiceAccountController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("Index/{type}")]
    public async Task<IActionResult> Index(string type, [FromQuery] VoucherQuery model)
    {
        if (type == null) return BadRequest();
        // TODO: verificar.
        var result = from m in _context.InvoiceAccounts select m;
        // var result = from m in _context.InvoiceAccounts.Where(m =>
        //         m.AccountType.Equals(type.ToUpper()) && m.Year.Equals(model.Year) && m.Month.Equals(model.Month))
        //     select m;
        if (!string.IsNullOrWhiteSpace(model.Query))
            result = result.Where(m => m.Status.Equals(model.Query.ToUpper()));
        result = result.OrderByDescending(m => m.EndDate);
        var responseData = await result.AsNoTracking().ToListAsync();
        return Ok(responseData);
    }
}
