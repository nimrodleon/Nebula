using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser() { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest("Username or password invalid");
            return Ok(new { Ok = true, User = user });
        }
    }
}
