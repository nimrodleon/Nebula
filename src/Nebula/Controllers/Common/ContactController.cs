using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Helpers;
using Nebula.Data.Models.Common;
using Nebula.Data.Services.Common;
using Nebula.Data.ViewModels.Common;

namespace Nebula.Controllers.Common;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly ContactService _contactService;

    public ContactController(ContactService contactService) =>
        _contactService = contactService;

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _contactService.GetListAsync(query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var contact = await _contactService.GetAsync(id);
        return Ok(contact);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2([FromQuery] string? term)
    {
        var responseData = await _contactService.GetListAsync(term, 10);
        var data = new List<Select2>();
        responseData.ForEach(item =>
        {
            data.Add(new Select2() {Id = item.Id, Text = $"{item.Document} - {item.Name}"});
        });
        return Ok(new {Results = data});
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Contact model)
    {
        model.Name = model.Name.ToUpper();
        await _contactService.CreateAsync(model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"El contacto {model.Name} ha sido registrado!"
        });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Contact model)
    {
        var contact = await _contactService.GetAsync(id);
        model.Id = contact.Id;
        model.Name = model.Name.ToUpper();
        await _contactService.UpdateAsync(id, model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"El contacto {model.Name} ha sido actualizado!"
        });
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var contact = await _contactService.GetAsync(id);
        await _contactService.RemoveAsync(id);
        return Ok(new {Ok = true, Data = contact, Msg = "El contacto ha sido borrado!"});
    }
}
