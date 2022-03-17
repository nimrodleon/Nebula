using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Helpers;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRavenDbContext _context;

        public UserController(IRavenDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserRegister model)
        {
            using var session = _context.Store.OpenAsyncSession();
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = PasswordHasher.HashPassword(model.Password)
            };

            await session.StoreAsync(user);
            await session.SaveChangesAsync();

            return Ok(new {Ok = true, User = user});
        }
    }
}
