using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Entities.EntitiesCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class EntityController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(EntitiesCreateRequest request)
        {
            return Ok(await _mediator.Send(new CreateEntitiesCommandRequest(new EntitiesBasicInfoRequest<EntitiesCreateRequest>(request))));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok(await _mediator.Send(new GetByCodeEntitiesCommandRequest(new GetByCodeEntitiesRequest { Code = code })));
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok(await _mediator.Send(new GetByTypeEntitiesCommandRequest(new GetByTypeEntitiesRequest { Type = type })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(EntitiesGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedEntitiesCommandRequest(request)));
        }
    }
}
