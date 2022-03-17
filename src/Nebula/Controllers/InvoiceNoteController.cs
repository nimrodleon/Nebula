using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Services;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceNoteController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IComprobanteService _comprobanteService;

        public InvoiceNoteController(ILogger<InvoiceNoteController> logger,
            ApplicationDbContext context, IComprobanteService comprobanteService)
        {
            _logger = logger;
            _context = context;
            _comprobanteService = comprobanteService;
        }

        [HttpGet("Index/{type}")]
        public async Task<IActionResult> Index(string type, [FromQuery] VoucherQuery model)
        {
            if (type == null) return BadRequest();
            var result = from m in _context.InvoiceNotes.Where(m =>
                    m.InvoiceType.Equals(type) && m.Year.Equals(model.Year) && m.Month.Equals(model.Month))
                select m;
            if (!string.IsNullOrWhiteSpace(model.Query))
                result = result.Where(m => m.RznSocialUsuario.ToUpper().Contains(model.Query.ToUpper()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var result = await _context.InvoiceNotes.AsNoTracking()
                .Include(m => m.InvoiceNoteDetails)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] NotaComprobante model)
        {
            try
            {
                _comprobanteService.SetModel(model);
                var invoiceNote = await _comprobanteService.CreateNote();
                return Ok(new
                {
                    Ok = true, Data = model,
                    Msg = $"{invoiceNote.Serie} - {invoiceNote.Number} ha sido registrado!"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new {Ok = false, Msg = e.Message});
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NotaComprobante model)
        {
            try
            {
                _comprobanteService.SetModel(model);
                var invoiceNote = await _comprobanteService.UpdateNote(id);
                return Ok(new
                {
                    Ok = true, Data = model,
                    Msg = $"{invoiceNote.Serie} - {invoiceNote.Number} ha sido actualizado!"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new {Ok = false, Msg = e.Message});
            }
        }
    }
}
