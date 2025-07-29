using EnergomeraTestTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnergomeraTestTask.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FieldsController(IFieldService service) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok(service.GetAll());

        [HttpGet("{id}/size")]
        public IActionResult GetSize(string id) => Ok(service.GetSize(id));

        [HttpGet("{id}/distance")]
        public IActionResult GetDistance(string id, [FromQuery] double lat, [FromQuery] double lng) =>
            Ok(service.GetDistance(id, lat, lng));

        [HttpGet("contains")]
        public IActionResult Contains([FromQuery] double lat, [FromQuery] double lng) =>
            Ok(service.Contains(lat, lng));

        [HttpPost("reload")]
        public IActionResult Reload()
        {
            service.Reload();
            return Ok("Reloaded");
        }
    }
}