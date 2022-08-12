using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Services.Inventory;

namespace Nebula.Controllers.Inventory
{
    [Authorize(Roles = AuthRoles.User)]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationDetailController : ControllerBase
    {
        private readonly LocationDetailService _locationDetailService;

        public LocationDetailController(LocationDetailService locationDetailService)
        {
            _locationDetailService = locationDetailService;
        }

        [HttpGet("Index/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var responseData = await _locationDetailService.GetListAsync(id);
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            var locationDetail = await _locationDetailService.GetAsync(id);
            return Ok(locationDetail);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LocationDetail model)
        {
            var locationDetail = await _locationDetailService.CreateAsync(model);
            return Ok(locationDetail);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] LocationDetail model)
        {
            var locationDetail = await _locationDetailService.GetAsync(id);
            model.Id = locationDetail.Id;
            var responseData = await _locationDetailService.UpdateAsync(id, model);
            return Ok(responseData);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var locationDetail = await _locationDetailService.GetAsync(id);
            await _locationDetailService.RemoveAsync(locationDetail.Id);
            return Ok(locationDetail);
        }
    }
}
