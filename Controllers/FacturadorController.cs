using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturadorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client = new();

        public FacturadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<Configuration> GetConfiguration()
        {
            return await _context.Configuration.AsNoTracking().FirstAsync();
        }

        [HttpGet("ActualizarPantalla")]
        public async Task<IActionResult> ActualizarPantalla()
        {
            var config = await GetConfiguration();
            var data = JsonSerializer.Serialize(new { });
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{config.UrlApi}/api/ActualizarPantalla.htm", content);
            var result = await httpResponse.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpGet("EliminarBandeja")]
        public async Task<IActionResult> EliminarBandeja()
        {
            var config = await GetConfiguration();
            var data = JsonSerializer.Serialize(new { });
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{config.UrlApi}/api/EliminarBandeja.htm", content);
            var result = await httpResponse.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpGet("GenerarComprobante")]
        public async Task<IActionResult> GenerarComprobante([FromQuery] int invoice)
        {
            var config = await GetConfiguration();
            var comprobante = await _context.Invoices.AsNoTracking()
                .SingleAsync(m => m.Id.Equals(invoice));
            string tipDocu = string.Empty;
            if (comprobante.DocType.Equals("FT")) tipDocu = "01";
            if (comprobante.DocType.Equals("BL")) tipDocu = "03";
            var data = JsonSerializer.Serialize(new
            {
                num_ruc = config.Ruc,
                tip_docu = tipDocu,
                num_docu = $"{comprobante.Serie}-{comprobante.Number}"
            });
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{config.UrlApi}/api/GenerarComprobante.htm", content);
            var result = await httpResponse.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpGet("EnviarXML")]
        public async Task<IActionResult> EnviarXml([FromQuery] int invoice)
        {
            var config = await GetConfiguration();
            var comprobante = await _context.Invoices.AsNoTracking()
                .SingleAsync(m => m.Id.Equals(invoice));
            string tipDocu = string.Empty;
            if (comprobante.DocType.Equals("FT")) tipDocu = "01";
            if (comprobante.DocType.Equals("BL")) tipDocu = "03";
            var data = JsonSerializer.Serialize(new
            {
                num_ruc = config.Ruc,
                tip_docu = tipDocu,
                num_docu = $"{comprobante.Serie}-{comprobante.Number}"
            });
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{config.UrlApi}/api/enviarXML.htm", content);
            var result = await httpResponse.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpGet("GenerarPdf")]
        public async Task<IActionResult> GenerarPdf([FromQuery] string nomArch)
        {
            var config = await GetConfiguration();
            var data = JsonSerializer.Serialize(new {nomArch});
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{config.UrlApi}/api/MostrarXml.htm", content);
            var result = await httpResponse.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf([FromQuery] string nomArch)
        {
            var config = await GetConfiguration();
            var stream = new FileStream(Path.Combine(config.FileSunat,
                Path.Combine("REPO", $"{nomArch}.pdf")), FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
    }
}
