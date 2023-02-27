using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Dto.Sales;

namespace Nebula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        var leer = new LeerDigestValue();
        return Ok(new
        {
            Value = leer.GetValue(@"C:\facturador\sunat_archivos\sfs\FIRMA\20520485750-03-B001-00000007.xml")
        });
    }
}
