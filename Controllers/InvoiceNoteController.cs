using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    }
}
