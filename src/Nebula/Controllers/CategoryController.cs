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
    public class CategoryController : ControllerBase
    {
        private readonly IRavenDbContext _context;

        public CategoryController(IRavenDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string? query)
        {
            using var session = _context.Store.OpenAsyncSession();
            IRavenQueryable<Category> categories = from m in session.Query<Category>() select m;
            if (!string.IsNullOrWhiteSpace(query))
                categories = categories.Search(m => m.Name, $"*{query.ToUpper()}*");
            var responseData = await categories.Take(25).ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            Category category = await session.LoadAsync<Category>(id);
            return Ok(category);
        }

        [HttpGet("Select2")]
        public async Task<IActionResult> Select2([FromQuery] string term)
        {
            using var session = _context.Store.OpenAsyncSession();
            IRavenQueryable<Category> categories = from m in session.Query<Category>() select m;
            if (!string.IsNullOrWhiteSpace(term))
                categories = categories.Search(m => m.Name, $"*{term.ToUpper()}*");
            var responseData = await categories.Take(10).ToListAsync();
            var data = new List<Select2>();
            responseData.ForEach(item =>
            {
                data.Add(new Select2()
                {
                    Id = item.Id,
                    Text = item.Name
                });
            });
            return Ok(new { Results = data });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Category model)
        {
            using var session = _context.Store.OpenAsyncSession();
            // database will create a GUID value for it
            model.Id = string.Empty;
            model.Name = model.Name.ToUpper();
            await session.StoreAsync(model);
            await session.SaveChangesAsync();

            return Ok(new
            {
                Ok = true,
                Data = model,
                Msg = $"La Categoría {model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Category model)
        {
            if (id != model.Id) return BadRequest();
            using var session = _context.Store.OpenAsyncSession();
            Category category = await session.LoadAsync<Category>(id);
            category.Name = model.Name.ToUpper();
            await session.SaveChangesAsync();

            return Ok(new
            {
                Ok = true,
                Data = category,
                Msg = $"La Categoría {category.Name} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            Category category = await session.LoadAsync<Category>(id);
            session.Delete(category);
            await session.SaveChangesAsync();
            return Ok(new { Ok = true, Data = category, Msg = "La categoría ha sido borrado!" });
        }
    }
}
