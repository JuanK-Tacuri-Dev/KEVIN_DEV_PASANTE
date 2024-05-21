using Integration.Orchestrator.Backend.Api.Filter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.IntegrationV2ToV1Commands;

namespace Integration.Orchestrator.Backend.Api.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class IntegrationV2toV1Controller(IMediator mediator, ILogger<IntegrationV2toV1Controller> logger) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<IntegrationV2toV1Controller> _logger = logger;


        [HttpGet]
        public async Task<IActionResult> IntegrationV2ToV1()
        {
            var response = await _mediator.Send(new IntegrationV2toV1CommandRequest());
            if (response.response)
            {
                return Ok("Integración V2toV1 OK");
            }
            else
            {
                return NotFound("Integración V2toV1 Falló");
            }
        }
     
    }
}
