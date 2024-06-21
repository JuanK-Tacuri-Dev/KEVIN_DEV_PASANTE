using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Property;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class PropertyController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(PropertyCreateRequest request)
        {
            return Ok(await _mediator.Send(new CreatePropertyCommandRequest(new PropertyBasicInfoRequest<PropertyCreateRequest>(request))));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok(await _mediator.Send(new GetByCodePropertyCommandRequest(new GetByCodePropertyRequest { Code = code })));
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok(await _mediator.Send(new GetByTypePropertyCommandRequest(new GetByTypePropertyRequest { Type = type })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(PropertyGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedPropertyCommandRequest(request)));
        }
    }
}
