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
        // GET: api/bienvenida
        [HttpGet("Bienvenida")]
        public IActionResult GetBienvenida()
        {
            return Ok("¡Bienvenido a mi API, desarrollada por KEVIN!");
        }
        // GET: api/respuesta
        [HttpGet("MensajeAleatorio")]
        public IActionResult GetMensajeAleatorio()
        {
            var mensajes = new[] { "¡Hola!", "¡Buenos días!", "¡Saludos!" };
            var random = new Random();
            var mensajeAleatorio = mensajes[random.Next(mensajes.Length)];
            return Ok(mensajeAleatorio);
        }

        // POST: api/Saludo
        [HttpPost]
        public IActionResult PostSaludo([FromBody] string nombre)
        {
            return Ok($"¡Saludos, {nombre}!");
        }
        // POST: api/Saludo/EnviarObjeto
        [HttpPost("EnviarObjeto")]
        public IActionResult EnviarObjeto([FromBody] object objeto)
        {
            return Ok(objeto);
        }


        // GET: api/Saludo/{nombre}
        [HttpGet("{nombre}")]
        public IActionResult GetSaludoPersonalizado(string nombre)
        {
            return Ok($"¡Hola, {nombre}!");
        }
        // GET: api/Saludo/ListaSaludos
        [HttpGet("ListaSaludos")]
        public IActionResult GetListaSaludos()
        {
            var saludos = new List<string> { "¡Hola!", "¡Buenos días!", "¡Buenas tardes!", "¡Buenas noches!" };
            return Ok(saludos);
        }

    }
}
