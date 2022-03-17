using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferNoteController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public TransferNoteController(ILogger<TransferNoteDetail> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] TransferFilter filter)
        {
            var result = await _context.TransferNotes.AsNoTracking()
                .Include(m => m.Origin)
                .Include(m => m.Target)
                .Where(m => m.OriginId.Equals(filter.Origin)
                            && m.TargetId.Equals(filter.Target) && m.Year.Equals(filter.Year)
                            && m.Month.Equals(filter.Month))
                .OrderByDescending(m => m.Id).ToListAsync();
            return Ok(result);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var result = await _context.TransferNotes.IgnoreQueryFilters().AsNoTracking()
                .Include(m => m.TransferNoteDetails).FirstOrDefaultAsync(m => m.Id.Equals(id));
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Transfer model)
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var motivo = await _context.InventoryReasons.AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id.Equals(model.Motivo));

                    // Cabecera Transferencia.
                    var transfer = new TransferNote()
                    {
                        OriginId = model.Origin,
                        TargetId = model.Target,
                        Motivo = $"{motivo.Id}|{motivo.Description}",
                        StartDate = model.StartDate,
                        Status = "BORRADOR",
                        Year = model.StartDate.ToString("yyyy"),
                        Month = model.StartDate.ToString("MM")
                    };
                    _context.TransferNotes.Add(transfer);
                    await _context.SaveChangesAsync();

                    // agregar detalle de Nota.
                    var transferNoteDetails = new List<TransferNoteDetail>();
                    model.ItemNotes.ForEach(item =>
                    {
                        transferNoteDetails.Add(new TransferNoteDetail()
                        {
                            TransferNoteId = transfer.Id,
                            ProductId = item.ProductId,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Amount = item.Amount
                        });
                    });
                    _context.TransferNoteDetails.AddRange(transferNoteDetails);
                    await _context.SaveChangesAsync();

                    // confirmar transacción.
                    await transaction.CommitAsync();
                    _logger.LogInformation("Transacción confirmada!");

                    return Ok(new
                    {
                        Ok = true, Data = model,
                        Msg = $"La Transferencia {transfer.Id}, ha sido registrado!"
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
                Ok = false, Msg = "Hubo un error en la emisión de la Transferencia!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Transfer model)
        {
            var result = await _context.TransferNotes.AsNoTracking()
                .Include(m => m.TransferNoteDetails)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest(new {Ok = false, Msg = "No existe la Transferencia de Inventario!"});
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var motivo = await _context.InventoryReasons.AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id.Equals(model.Motivo));

                    // Editar Cabecera.
                    result.OriginId = model.Origin;
                    result.TargetId = model.Target;
                    result.Motivo = $"{motivo.Id}|{motivo.Description}";
                    result.StartDate = model.StartDate;
                    result.Status = "BORRADOR";
                    result.Year = model.StartDate.ToString("yyyy");
                    result.Month = model.StartDate.ToString("MM");
                    _context.TransferNotes.Update(result);
                    await _context.SaveChangesAsync();

                    // borrar todos los items anteriores.
                    _context.TransferNoteDetails.RemoveRange(result.TransferNoteDetails);
                    await _context.SaveChangesAsync();

                    // agregar detalle de Nota.
                    var transferNoteDetails = new List<TransferNoteDetail>();
                    model.ItemNotes.ForEach(item =>
                    {
                        transferNoteDetails.Add(new TransferNoteDetail()
                        {
                            TransferNoteId = result.Id,
                            ProductId = item.ProductId,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Amount = item.Amount
                        });
                    });
                    _context.TransferNoteDetails.AddRange(transferNoteDetails);
                    await _context.SaveChangesAsync();

                    // confirmar transacción.
                    await transaction.CommitAsync();
                    _logger.LogInformation("Transacción confirmada!");

                    return Ok(new
                    {
                        Ok = true, Data = model,
                        Msg = $"La Transferencia {result.Id}, ha sido actualizado!"
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
                Ok = false, Msg = "Hubo un error en la actualización de la Transferencia!"
            });
        }
    }
}
