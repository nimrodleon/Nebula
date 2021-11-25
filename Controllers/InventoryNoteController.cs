using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
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

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null) return BadRequest();
            var result = await _context.InventoryNotes.IgnoreQueryFilters().AsNoTracking()
                .Include(m => m.InventoryNoteDetails).FirstOrDefaultAsync(m => m.Id.Equals(id));
            return Ok(result);
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] Note model)
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var note = new InventoryNote()
                    {
                        ContactId = model.ContactId,
                        WarehouseId = model.WarehouseId,
                        Motivo = model.Motivo,
                        StartDate = model.StartDate,
                        Remark = model.Remark,
                        Status = "BORRADOR"
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
                        Ok = true, Data = model,
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
                Ok = false, Msg = "Hubo un error en la emisión de la Nota!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int? id, [FromBody] Note model)
        {
            var result = await _context.InventoryNotes.FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest(new {Ok = false, Msg = "No existe la Nota de Inventario!"});
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    result.ContactId = model.ContactId;
                    result.WarehouseId = model.WarehouseId;
                    result.Motivo = model.Motivo;
                    result.StartDate = model.StartDate;
                    result.Remark = model.Remark;
                    result.Status = "BORRADOR";
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
                        Ok = true, Data = model,
                        Msg = $"La Nota {result.Id}, ha sido registrado!"
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
                Ok = false, Msg = "Hubo un error en la emisión de la Nota!"
            });
        }
    }
}