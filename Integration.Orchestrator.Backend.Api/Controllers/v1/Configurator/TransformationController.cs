using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurator.Transformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Transformation.TransformationCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurator
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/transformation/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class TransformationController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(TransformationGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedTransformationCommandRequest(request))).Message);
        }
    }
}
