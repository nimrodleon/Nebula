using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Services;
using Nebula.Data.Services.Cashier;
using Nebula.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Nebula.Data.Helpers;

namespace Nebula.Controllers
{
    [Authorize(Roles = AuthRoles.User)]
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivableController : ControllerBase
    {
        private readonly ReceivableService _receivableService;
        private readonly CashierDetailService _cashierDetailService;

        public ReceivableController(ReceivableService receivableService, CashierDetailService cashierDetailService)
        {
            _receivableService = receivableService;
            _cashierDetailService = cashierDetailService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string month, [FromQuery] string year, [FromQuery] string status)
        {
            var responseData = await _receivableService.GetListAsync(month, year, status);
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            var receivable = await _receivableService.GetAsync(id);
            return Ok(receivable);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Receivable model)
        {
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
            var receivable = await _receivableService.GetAsync(id);

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
            var receivable = await _receivableService.GetAsync(id);
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
    }
}
