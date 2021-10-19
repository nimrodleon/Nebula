using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public UserController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("UserAdminRegister")]
        public IActionResult UserAdminRegister([FromBody] UserRegister model)
        {
            if (!_configuration.GetValue<bool>("UserAdminRegister"))
            {
                return NotFound(new { Ok = false, Msg = "Operación no valida!" });
            }

            // validar nombre del usuario.
            if (_context.Users.Any(m =>
                m.UserName.Equals(model.UserName) && m.SoftDeleted.Equals(false)))
                return NotFound(new { Ok = false, Msg = "El nombre del usuario ya existe!" });

            var user = new User()
            {
                Name = model.Name,
                UserName = model.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "ROLE_ADMIN",
                Email = model.Email,
                Suspended = false,
                SoftDeleted = false
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(new { Ok = true, User = user });
        }
    }
}
