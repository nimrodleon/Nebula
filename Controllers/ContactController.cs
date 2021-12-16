using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Helpers;
using Nebula.Data.Models;
using Nebula.Data.Services;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUriService _uriService;

        public ContactController(ApplicationDbContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.Query);
            var skip = (validFilter.PageNumber - 1) * validFilter.PageSize;
            var result = from c in _context.Contacts select c;
            if (!string.IsNullOrWhiteSpace(filter.Query))
                result = result.Where(m => m.Document.Contains(filter.Query)
                                           || m.Name.ToLower().Contains(filter.Query.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var pagedData = await result.AsNoTracking().Skip(skip).Take(validFilter.PageSize).ToListAsync();
            var totalRecords = await result.CountAsync();
            var pagedResponse =
                PaginationHelper.CreatePagedResponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedResponse);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null) return BadRequest();
            var result = await _context.Contacts.IgnoreQueryFilters()
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpGet("Select2")]
        public async Task<IActionResult> Select2([FromQuery] string term)
        {
            var result = from c in _context.Contacts select c;
            if (!string.IsNullOrWhiteSpace(term))
                result = result.Where(m =>
                    m.Document.Contains(term) || m.Name.ToLower().Contains(term.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().Take(10).ToListAsync();
            var data = new List<Select2>();
            responseData.ForEach(item =>
            {
                data.Add(new Select2() {Id = item.Id, Text = $"{item.Document} - {item.Name}"});
            });
            return Ok(new {Results = data});
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Contact model)
        {
            _context.Contacts.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Document} - {model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int? id, [FromBody] Contact model)
        {
            if (id != model.Id) return BadRequest();
            _context.Contacts.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Document} - {model.Name} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _context.Contacts.FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            _context.Contacts.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new {Ok = true, Data = result, Msg = "El contacto ha sido borrado!"});
        }
    }
}
