using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Helpers;
using Nebula.Data.Models;
using Nebula.Data.Services;

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
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Contacts.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToListAsync();
            var totalRecords = await _context.Contacts.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedResponse<Contact>
                (pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedResponse);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Contact model)
        {
            model.SoftDeleted = false;
            _context.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, Contact = model });
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id, [FromBody] Contact model)
        {
            if (id != model.Id) return BadRequest();
            model.SoftDeleted = false;
            _context.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, Contact = model });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (contact == null) return NotFound();
            contact.SoftDeleted = true;
            _context.Update(contact);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, Msg = "Contacto borrado!" });
        }
    }
}
