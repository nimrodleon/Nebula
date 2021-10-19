using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var contacts = await _context.Contacts.ToListAsync();
            if (contacts == null) return NotFound();
            return Ok(contacts);
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "No se puede guardar los cambios." +
                                             "Vuelve a intentarlo y si el problema persiste, consulte con el administrador del sistema");
                return NotFound(new { Ok = false, Contact = model });
            }

            model.SoftDeleted = false;
            _context.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, Contact = model });
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id, [FromBody] Contact model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "No se puede guardar los cambios." +
                                             "Vuelve a intentarlo y si el problema persiste, consulte con el administrador del sistema");
                return NotFound(new { Ok = false, Contact = model });
            }

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
