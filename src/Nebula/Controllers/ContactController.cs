using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IRavenDbContext _context;

        public ContactController(IRavenDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            using var session = _context.Store.OpenAsyncSession();
            IRavenQueryable<Contact> contacts = from m in session.Query<Contact>() select m;
            if (!string.IsNullOrWhiteSpace(query))
                contacts = contacts.Search(m => m.Name, $"*{query.ToUpper()}*");
            var responseData = await contacts.Take(25).ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            Contact contact = await session.LoadAsync<Contact>(id);
            return Ok(contact);
        }

        [HttpGet("Select2")]
        public async Task<IActionResult> Select2([FromQuery] string term)
        {
            using var session = _context.Store.OpenAsyncSession();
            IRavenQueryable<Contact> contacts = from m in session.Query<Contact>() select m;
            if (!string.IsNullOrWhiteSpace(term))
                contacts = contacts.Search(m => m.Name, $"*{term.ToUpper()}*");
            var responseData = await contacts.Take(10).ToListAsync();
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
            using var session = _context.Store.OpenAsyncSession();
            model.Id = string.Empty;
            model.Name = model.Name.ToUpper();
            await session.StoreAsync(model);
            await session.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"El contacto {model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Contact model)
        {
            if (id != model.Id) return BadRequest();
            using var session = _context.Store.OpenAsyncSession();
            Contact contact = await session.LoadAsync<Contact>(id);
            contact.Document = model.Document;
            contact.DocType = model.DocType;
            contact.Name = model.Name.ToUpper();
            contact.Address = model.Address;
            contact.PhoneNumber = model.PhoneNumber;
            contact.Email = model.Email;
            await session.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = contact,
                Msg = $"El contacto {contact.Name} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            Contact contact = await session.LoadAsync<Contact>(id);
            session.Delete(contact);
            await session.SaveChangesAsync();
            return Ok(new {Ok = true, Data = contact, Msg = "El contacto ha sido borrado!"});
        }
    }
}
