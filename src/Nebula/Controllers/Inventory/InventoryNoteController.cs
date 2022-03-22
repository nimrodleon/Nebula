using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class InventoryNoteController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ApplicationDbContext _context;

    public InventoryNoteController(ILogger<InventoryNoteController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] NoteFilter filter)
    {
        var result = await _context.InventoryNotes.AsNoTracking()
            .Include(m => m.Contact)
            .Include(m => m.Warehouse)
            .Where(m => m.WarehouseId.ToString().Equals(filter.WarehouseId)
                        && m.NoteType.Equals(filter.NoteType.ToUpper()) && m.Year.Equals(filter.Year)
                        && m.Month.Equals(filter.Month))
            .OrderByDescending(m => m.Id).ToListAsync();
        return Ok(result);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(int? id)
    {
        if (id == null) return BadRequest();
        var result = await _context.InventoryNotes.IgnoreQueryFilters().AsNoTracking()
            .Include(m => m.Contact)
            .Include(m => m.InventoryNoteDetails)
            .FirstOrDefaultAsync(m => m.Id.Equals(id));
        return Ok(result);
    }

    [HttpPost("Store")]
    public async Task<IActionResult> Store([FromBody] Note model)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var motivo = await _context.InventoryReasons.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id.Equals(model.Motivo));

                // Cabecera Nota.
                var note = new InventoryNote()
                {
                    ContactId = model.ContactId,
                    WarehouseId = model.WarehouseId,
                    NoteType = model.NoteType.ToUpper(),
                    Motivo = $"{motivo.Id}|{motivo.Description}",
                    StartDate = model.StartDate,
                    Status = "BORRADOR",
                    Year = model.StartDate.ToString("yyyy"),
                    Month = model.StartDate.ToString("MM")
                };
                _context.InventoryNotes.Add(note);
                await _context.SaveChangesAsync();

                // agregar detalle de Nota.
                var inventoryNoteDetails = new List<InventoryNoteDetail>();
                model.ItemNotes.ForEach(item =>
                {
                    inventoryNoteDetails.Add(new InventoryNoteDetail()
                    {
                        InventoryNote = note,
                        ProductId = item.ProductId,
                        Description = item.Description,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Amount = item.Amount
                    });
                });
                _context.InventoryNoteDetails.AddRange(inventoryNoteDetails);
                await _context.SaveChangesAsync();

                // confirmar transacción.
                await transaction.CommitAsync();
                _logger.LogInformation("Transacción confirmada!");

                return Ok(new
                {
                    Ok = true,
                    Data = model,
                    Msg = $"La Nota {note.Id}, ha sido registrado!"
                });
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("La transacción ha sido cancelada!");
                _logger.LogError(e.Message);
            }
        }

        return BadRequest(new
        {
            Ok = false,
            Msg = "Hubo un error en la emisión de la Nota!"
        });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int? id, [FromBody] Note model)
    {
        var result = await _context.InventoryNotes.Include(m => m.InventoryNoteDetails)
            .FirstOrDefaultAsync(m => m.Id.Equals(id));
        if (result == null) return BadRequest(new { Ok = false, Msg = "No existe la Nota de Inventario!" });
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var motivo = await _context.InventoryReasons.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id.Equals(model.Motivo));

                // Editar Cabecera.
                result.ContactId = model.ContactId;
                result.WarehouseId = model.WarehouseId;
                result.NoteType = model.NoteType.ToUpper();
                result.Motivo = $"{motivo.Id}|{motivo.Description}";
                result.StartDate = model.StartDate;
                result.Status = "BORRADOR";
                result.Year = model.StartDate.ToString("yyyy");
                result.Month = model.StartDate.ToString("MM");
                _context.InventoryNotes.Update(result);
                await _context.SaveChangesAsync();

                // borrar todos los items anteriores.
                _context.InventoryNoteDetails.RemoveRange(result.InventoryNoteDetails);
                await _context.SaveChangesAsync();

                // agregar detalle de Nota.
                var inventoryNoteDetails = new List<InventoryNoteDetail>();
                model.ItemNotes.ForEach(item =>
                {
                    inventoryNoteDetails.Add(new InventoryNoteDetail()
                    {
                        InventoryNoteId = result.Id,
                        ProductId = item.ProductId,
                        Description = item.Description,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Amount = item.Amount
                    });
                });
                _context.InventoryNoteDetails.AddRange(inventoryNoteDetails);
                await _context.SaveChangesAsync();

                // confirmar transacción.
                await transaction.CommitAsync();
                _logger.LogInformation("Transacción confirmada!");

                return Ok(new
                {
                    Ok = true,
                    Data = model,
                    Msg = $"La Nota {result.Id}, ha sido actualizado!"
                });
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("La transacción ha sido cancelada!");
                _logger.LogError(e.Message);
            }
        }

        return BadRequest(new
        {
            Ok = false,
            Msg = "Hubo un error en la actualización de la Nota!"
        });
    }
}
