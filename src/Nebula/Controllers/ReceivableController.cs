using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Cashier;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Configurations;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Finanzas.Dto;

namespace Nebula.Controllers
{
    [Authorize(Roles = AuthRoles.User)]
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivableController : ControllerBase
    {
        private readonly ReceivableService _receivableService;
        private readonly CashierDetailService _cashierDetailService;
        private readonly ConfigurationService _configurationService;

        public ReceivableController(ReceivableService receivableService,
            CashierDetailService cashierDetailService, ConfigurationService configurationService)
        {
            _receivableService = receivableService;
            _cashierDetailService = cashierDetailService;
            _configurationService = configurationService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] ReceivableRequestParams requestParam)
        {
            var responseData = await _receivableService.GetListAsync(requestParam);
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            var receivable = await _receivableService.GetByIdAsync(id);
            return Ok(receivable);
        }

        [HttpGet("GetReceivablesByContactId/{contactId}")]
        public async Task<IActionResult> GetReceivablesByContactId(string contactId,
            [FromQuery] ReceivableRequestParams requestParam)
        {
            var receivable = await _receivableService.GetReceivablesByContactId(contactId, requestParam);
            return Ok(receivable);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Receivable model)
        {
            var license = await _configurationService.ValidarAcceso();
            if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });

            await _receivableService.CreateAsync(_cashierDetailService, model);

            return Ok(new
            {
                Ok = true,
                Data = model,
                Msg = $"El {model.Type.ToLower()} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Receivable model)
        {
            var license = await _configurationService.ValidarAcceso();
            if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });

            var receivable = await _receivableService.GetByIdAsync(id);

            model.Id = receivable.Id;
            await _receivableService.UpdateAsync(id, model);

            return Ok(new
            {
                Ok = true,
                Data = model,
                Msg = $"El {model.Type.ToLower()} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            var license = await _configurationService.ValidarAcceso();
            if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
            var receivable = await _receivableService.GetByIdAsync(id);
            if (receivable.Type == "CARGO")
                await _receivableService.RemoveAsync(receivable.Id);
            if (receivable.Type == "ABONO")
                await _receivableService.RemoveAbonoAsync(receivable);
            return Ok(new { Ok = true, Data = receivable, Msg = $"El {receivable.Type.ToLower()} ha sido borrado!" });
        }

        [HttpGet("Abonos/{id}")]
        public async Task<IActionResult> Abonos(string id)
        {
            var receivables = await _receivableService.GetAbonosAsync(id);
            return Ok(receivables);
        }

        [HttpGet("TotalAbonos/{id}")]
        public async Task<IActionResult> TotalAbonos(string id)
        {
            var total = await _receivableService.GetTotalAbonosAsync(id);
            return Ok(total);
        }

        [AllowAnonymous]
        [HttpGet("ExportDeudaExcel/{contactId}")]
        public async Task<IActionResult> ExportDeudaExcel(string contactId, [FromQuery] string year)
        {
            List<Receivable> cuentasPorCobrar = await _receivableService.GetReceivablesByContactId(contactId, year);
            ExportarCuentasPorCobrarDto exportar = new ExportarCuentasPorCobrarDto(cuentasPorCobrar);
            string pathExcel = exportar.GenerarArchivoExcel();
            FileStream stream = new FileStream(pathExcel, FileMode.Open);
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
