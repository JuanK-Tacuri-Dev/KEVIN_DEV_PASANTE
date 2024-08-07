using Integration.Orchestrator.Backend.Api.Filter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Integrations.IntegrationV1ToV2Commands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Integration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class IntegrationV1toV2Controller(IMediator mediator, ILogger<IntegrationV1toV2Controller> logger) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<IntegrationV1toV2Controller> _logger = logger;


        [HttpGet]
        public async Task<IActionResult> IntegrationV1ToV2()
        {
            var response = await _mediator.Send(new IntegrationV1toV2CommandRequest());
            if (response.response)
            {
                return Ok("Integración V1toV2 OK");
            }
            else
            {
                return NotFound("Integración V1toV2 Falló");
            }
        }

    }
}
