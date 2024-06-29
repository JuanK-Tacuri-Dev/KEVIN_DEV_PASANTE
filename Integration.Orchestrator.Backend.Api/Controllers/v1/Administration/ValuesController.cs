using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Value;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Value.ValueCommands;

namespace Value.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class ValuesController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(ValueCreateRequest request)
        {
            return Ok(await _mediator.Send(
                new CreateValueCommandRequest(
                    new ValueBasicInfoRequest<ValueCreateRequest>(request))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ValueUpdateRequest request, Guid id)
        {
            return Ok(await _mediator.Send(
                new UpdateValueCommandRequest(
                    new ValueBasicInfoRequest<ValueUpdateRequest>(request), id)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(
                new DeleteValueCommandRequest(
                    new ValueDeleteRequest { Id = id })));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _mediator.Send(
                new GetByIdValueCommandRequest(
                    new ValueGetByIdRequest { Id = id })));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok(await _mediator.Send(
                new GetByCodeValueCommandRequest(
                    new ValueGetByCodeRequest { Code = code })));
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok(await _mediator.Send(
                new GetByTypeValueCommandRequest(
                    new ValueGetByTypeRequest { Type = type })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(ValueGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(
                new GetAllPaginatedValueCommandRequest(request)));
        }
    }
}
