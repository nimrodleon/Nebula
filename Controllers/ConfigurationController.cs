using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IRavenDbContext _context;

        public ConfigurationController(IRavenDbContext context)
        {
            _context = context;
        }

        [HttpGet("Show")]
        public async Task<IActionResult> Show()
        {
            using var session = _context.Store.OpenAsyncSession();
            Configuration configuration = await session.LoadAsync<Configuration>("default");
            if (configuration == null)
            {
                var model = new Configuration()
                {
                    Id = "default"
                };
                await session.StoreAsync(model);
                await session.SaveChangesAsync();
                return RedirectToAction("Show");
            }

            return Ok(configuration);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Configuration model)
        {
            using var session = _context.Store.OpenAsyncSession();
            Configuration configuration = await session.LoadAsync<Configuration>("default");
            configuration.Ruc = model.Ruc;
            configuration.RznSocial = model.RznSocial.ToUpper();
            configuration.CodLocalEmisor = model.CodLocalEmisor;
            configuration.TipMoneda = model.TipMoneda;
            configuration.PorcentajeIgv = model.PorcentajeIgv;
            configuration.ValorImpuestoBolsa = model.ValorImpuestoBolsa;
            configuration.CpeSunat = model.CpeSunat;
            configuration.ContactId = model.ContactId;
            configuration.UrlApi = model.UrlApi;
            configuration.FileSunat = model.FileSunat;
            configuration.FileControl = model.FileControl;
            await session.SaveChangesAsync();

            return Ok(new
            {
                Ok = true, Data = configuration,
                Msg = $"La configuración {configuration.Ruc}, ha sido actualizado!"
            });
        }
    }
}
