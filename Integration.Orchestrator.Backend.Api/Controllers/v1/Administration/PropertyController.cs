using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Property;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Property.PropertyCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/properties/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class PropertyController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(PropertyCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreatePropertyCommandRequest(
                    new PropertyBasicInfoRequest<PropertyCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PropertyUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(
                new UpdatePropertyCommandRequest(
                    new PropertyBasicInfoRequest<PropertyUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeletePropertyCommandRequest(
                    new PropertyDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdPropertyCommandRequest(
                    new PropertyGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok((await _mediator.Send(
                new GetByCodePropertyCommandRequest(
                    new PropertyGetByCodeRequest { Code = code }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByType(Guid typeId)
        {
            return Ok((await _mediator.Send(
                new GetByTypePropertyCommandRequest(
                    new PropertyGetByTypeRequest { TypeId = typeId}))).Message);
        }
        

        [HttpGet]
        public async Task<IActionResult> GetByEntityId(Guid entityId)
        {
            return Ok((await _mediator.Send(
                new GetByEntityPropertyCommandRequest(
                    new PropertyGetByEntityRequest { EntityId = entityId }))).Message);
        }        

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(PropertyGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedPropertyCommandRequest(request))).Message);
        }
    }
}
