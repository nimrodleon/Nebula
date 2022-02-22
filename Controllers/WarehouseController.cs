using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IRavenDbContext _context;

        public WarehouseController(IRavenDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            using var session = _context.Store.OpenAsyncSession();
            IRavenQueryable<Warehouse> warehouses = from m in session.Query<Warehouse>() select m;
            if (!string.IsNullOrWhiteSpace(query))
                warehouses = warehouses.Search(m => m.Name, $"*{query.ToUpper()}*");
            var responseData = await warehouses.Take(25).ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            Warehouse warehouse = await session.LoadAsync<Warehouse>(id);
            return Ok(warehouse);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Warehouse model)
        {
            using var session = _context.Store.OpenAsyncSession();
            model.Id = string.Empty;
            model.Name = model.Name.ToUpper();
            await session.StoreAsync(model);
            await session.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"El Almacén {model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Warehouse model)
        {
            if (id != model.Id) return BadRequest();
            using var session = _context.Store.OpenAsyncSession();
            Warehouse warehouse = await session.LoadAsync<Warehouse>(id);
            warehouse.Name = model.Name.ToUpper();
            warehouse.Remark = model.Remark;
            await session.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = warehouse,
                Msg = $"El Almacén {warehouse.Name} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            Warehouse warehouse = await session.LoadAsync<Warehouse>(id);
            session.Delete(warehouse);
            await session.SaveChangesAsync();
            return Ok(new {Ok = true, Data = warehouse, Msg = "El almacén ha sido borrado!"});
        }
    }
}
