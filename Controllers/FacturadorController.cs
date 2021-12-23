using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturadorController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client = new();

        public FacturadorController(ILogger<FacturadorController> logger, ApplicationDbContext context)
        {
            _logger = logger;
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
                .FirstOrDefaultAsync(m => m.Id.Equals(invoice));
            _logger.LogInformation(JsonSerializer.Serialize(comprobante));
            string tipDocu = string.Empty;
            if (comprobante.DocType.Equals("FT")) tipDocu = "01";
            if (comprobante.DocType.Equals("BL")) tipDocu = "03";
            var data = JsonSerializer.Serialize(new
            {
                num_ruc = config.Ruc,
                tip_docu = tipDocu,
                num_docu = $"{comprobante.Serie}-{comprobante.Number}"
            });
            _logger.LogInformation(JsonSerializer.Serialize(data));
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{config.UrlApi}/api/GenerarComprobante.htm", content);
            var result = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogInformation(JsonSerializer.Serialize(result));
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
        public async Task<IActionResult> GenerarPdf([FromQuery] int invoice)
        {
            var config = await GetConfiguration();
            var comprobante = await _context.Invoices.AsNoTracking()
                .SingleAsync(m => m.Id.Equals(invoice));
            _logger.LogInformation(JsonSerializer.Serialize(comprobante));
            string tipDocu = string.Empty;
            if (comprobante.DocType.Equals("FT")) tipDocu = "01";
            if (comprobante.DocType.Equals("BL")) tipDocu = "03";
            // 20520485750-03-B001-00000015
            string nomArch = $"{config.Ruc}-{tipDocu}-{comprobante.Serie}-{comprobante.Number}";
            _logger.LogInformation(JsonSerializer.Serialize(nomArch));
            var data = JsonSerializer.Serialize(new {nomArch});
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"{config.UrlApi}/api/MostrarXml.htm", content);
            var result = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogInformation(JsonSerializer.Serialize(result));
            return Ok(JsonSerializer.Serialize(result));
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf([FromQuery] int invoice)
        {
            string pathFilePdf = string.Empty;
            var config = await GetConfiguration();
            var comprobante = await _context.Invoices.AsNoTracking()
                .SingleAsync(m => m.Id.Equals(invoice));
            string tipDocu = string.Empty;
            if (comprobante.DocType.Equals("FT")) tipDocu = "01";
            if (comprobante.DocType.Equals("BL")) tipDocu = "03";
            // 20520485750-03-B001-00000015
            string nomArch = $"{config.Ruc}-{tipDocu}-{comprobante.Serie}-{comprobante.Number}";
            var file = Path.Combine("REPO", $"{nomArch}.pdf");
            if (System.IO.File.Exists(Path.Combine(config.FileSunat, file)))
                pathFilePdf = Path.Combine(config.FileSunat, file);
            if (System.IO.File.Exists(Path.Combine(config.FileControl, file)))
                pathFilePdf = Path.Combine(config.FileControl, file);
            var stream = new FileStream(pathFilePdf, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [HttpGet("Backup")]
        public async Task<IActionResult> Backup([FromQuery] int invoice)
        {
            var config = await GetConfiguration();
            var comprobante = await _context.Invoices.AsNoTracking()
                .SingleAsync(m => m.Id.Equals(invoice));
            string tipDocu = string.Empty;
            if (comprobante.DocType.Equals("FT")) tipDocu = "01";
            if (comprobante.DocType.Equals("BL")) tipDocu = "03";
            // 20520485750-03-B001-00000015
            string nomArch = $"{config.Ruc}-{tipDocu}-{comprobante.Serie}-{comprobante.Number}";
            var fileData = $"{nomArch}.json";
            var fileEnvio = $"{nomArch}.zip";
            var fileFirma = $"{nomArch}.xml";
            var fileParse = $"{nomArch}.xml";
            var fileRepo = $"{nomArch}.pdf";
            var fileRpta = $"R{nomArch}.zip";
            var fileTemp = $"{nomArch}.xml";

            // configurar rutas de destino.
            var targetData = Path.Combine(config.FileControl, "DATA");
            if (!Directory.Exists(targetData)) Directory.CreateDirectory(targetData);
            var targetEnvio = Path.Combine(config.FileControl, "ENVIO");
            if (!Directory.Exists(targetEnvio)) Directory.CreateDirectory(targetEnvio);
            var targetFirma = Path.Combine(config.FileControl, "FIRMA");
            if (!Directory.Exists(targetFirma)) Directory.CreateDirectory(targetFirma);
            var targetParse = Path.Combine(config.FileControl, "PARSE");
            if (!Directory.Exists(targetParse)) Directory.CreateDirectory(targetParse);
            var targetRepo = Path.Combine(config.FileControl, "REPO");
            if (!Directory.Exists(targetRepo)) Directory.CreateDirectory(targetRepo);
            var targetRpta = Path.Combine(config.FileControl, "RPTA");
            if (!Directory.Exists(targetRpta)) Directory.CreateDirectory(targetRpta);
            var targetTemp = Path.Combine(config.FileControl, "TEMP");
            if (!Directory.Exists(targetTemp)) Directory.CreateDirectory(targetTemp);

            // copia de seguridad de archivos.
            if (System.IO.File.Exists(Path.Combine(config.FileSunat, Path.Combine("DATA", fileData))))
            {
                System.IO.File.Copy(Path.Combine(config.FileSunat, Path.Combine("DATA",
                    fileData)), Path.Combine(targetData, fileData));
                System.IO.File.Delete(Path.Combine(config.FileSunat, Path.Combine("DATA", fileData)));
            }

            if (System.IO.File.Exists(Path.Combine(config.FileSunat, Path.Combine("ENVIO", fileEnvio))))
            {
                System.IO.File.Copy(Path.Combine(config.FileSunat, Path.Combine("ENVIO",
                    fileEnvio)), Path.Combine(targetEnvio, fileEnvio));
                System.IO.File.Delete(Path.Combine(config.FileSunat, Path.Combine("ENVIO", fileEnvio)));
            }

            if (System.IO.File.Exists(Path.Combine(config.FileSunat, Path.Combine("FIRMA", fileFirma))))
            {
                System.IO.File.Copy(Path.Combine(config.FileSunat, Path.Combine("FIRMA",
                    fileFirma)), Path.Combine(targetFirma, fileFirma));
                System.IO.File.Delete(Path.Combine(config.FileSunat, Path.Combine("FIRMA", fileFirma)));
            }

            if (System.IO.File.Exists(Path.Combine(config.FileSunat, Path.Combine("PARSE", fileParse))))
            {
                System.IO.File.Copy(Path.Combine(config.FileSunat, Path.Combine("PARSE",
                    fileParse)), Path.Combine(targetParse, fileParse));
                System.IO.File.Delete(Path.Combine(config.FileSunat, Path.Combine("PARSE", fileParse)));
            }

            if (System.IO.File.Exists(Path.Combine(config.FileSunat, Path.Combine("REPO", fileRepo))))
            {
                System.IO.File.Copy(Path.Combine(config.FileSunat, Path.Combine("REPO",
                    fileRepo)), Path.Combine(targetRepo, fileRepo));
                System.IO.File.Delete(Path.Combine(config.FileSunat, Path.Combine("REPO", fileRepo)));
            }

            if (System.IO.File.Exists(Path.Combine(config.FileSunat, Path.Combine("RPTA", fileRpta))))
            {
                System.IO.File.Copy(Path.Combine(config.FileSunat, Path.Combine("RPTA",
                    fileRpta)), Path.Combine(targetRpta, fileRpta));
                System.IO.File.Delete(Path.Combine(config.FileSunat, Path.Combine("RPTA", fileRpta)));
            }

            if (System.IO.File.Exists(Path.Combine(config.FileSunat, Path.Combine("TEMP", fileTemp))))
            {
                System.IO.File.Copy(Path.Combine(config.FileSunat, Path.Combine("TEMP",
                    fileTemp)), Path.Combine(targetTemp, fileTemp));
                System.IO.File.Delete(Path.Combine(config.FileSunat, Path.Combine("TEMP", fileTemp)));
            }

            return Ok(new {Ok = true, Msg = "Backup Completado!"});
        }
    }
}
