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
            var objc = new 
            {
                Mensaje ="¡Hola, mundo!",
                NombreDev= "KEVIN ..... ",

            };
            return Ok(objc);
        }
        // GET: api/Saludoo/Hora
        [HttpGet("Hora")]
        public IActionResult GetHora()
        {
            var horaActual = DateTime.Now.ToString("HH:mm:ss");
            return Ok($"La hora actual es: {horaActual}");
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
