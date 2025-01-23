using Microsoft.AspNetCore.Mvc;

namespace MiApiSencilla.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaludoController : ControllerBase
    {
        // GET: api/Saludo
        [HttpGet]
        public IActionResult GetSaludo()
        {
            return Ok("¡Hola, mundo!");
        }
        // POST: api/Saludo
        [HttpPost]
        public IActionResult PostSaludo([FromBody] string nombre)
        {
            return Ok($"¡Saludos, {nombre}!");
        }

        // GET: api/Saludo/{nombre}
        [HttpGet("{nombre}")]
        public IActionResult GetSaludoPersonalizado(string nombre)
        {
            return Ok($"¡Hola, {nombre}!");
        }
    }
}
