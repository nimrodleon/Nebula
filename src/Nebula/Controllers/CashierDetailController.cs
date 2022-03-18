using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Nebula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CashierDetailController : ControllerBase
{
    private readonly IRavenDbContext _context;

    public CashierDetailController(IRavenDbContext context)
    {
        _context = context;
    }

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(string id, [FromQuery] string? query)
    {
        using var session = _context.Store.OpenAsyncSession();
        IRavenQueryable<CashierDetail> cashierDetails = from m in session.Query<CashierDetail>()
                .Where(x => x.CajaDiaria == id)
                                                        select m;
        if (!string.IsNullOrWhiteSpace(query))
            cashierDetails = cashierDetails.Search(m => m.Contact, $"*{query.ToUpper()}*");
        var responseData = await cashierDetails.ToListAsync();
        return Ok(responseData);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CashierDetail model)
    {
        using var session = _context.Store.OpenAsyncSession();
        model.Id = string.Empty;
        model.TypeOperation = TypeOperation.CajaChica;
        model.FormaPago = "Contado";
        await session.StoreAsync(model);
        await session.SaveChangesAsync();
        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = "La operación ha sido registrado!"
        });
    }
}
