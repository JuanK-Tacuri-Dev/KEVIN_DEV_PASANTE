using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Entities.EntitiesCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurador
{
    [Route("api/v1/entities/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class EntityController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(EntitiesCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateEntitiesCommandRequest(
                    new EntitiesBasicInfoRequest<EntitiesCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(EntitiesUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(new UpdateEntitiesCommandRequest(
                new EntitiesBasicInfoRequest<EntitiesUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(new DeleteEntitiesCommandRequest(
                new EntitiesDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdEntitiesCommandRequest(
                    new EntitiesGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok((await _mediator.Send(
                new GetByCodeEntitiesCommandRequest(
                    new EntitiesGetByCodeRequest { Code = code }))).Message);
        }
        [HttpGet]
        public async Task<IActionResult> GetByTypeId(Guid typeId)
        {
            return Ok((await _mediator.Send(
                new GetByTypeEntitiesCommandRequest(
                    new EntitiesGetByTypeRequest { TypeId = typeId }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByRepositoryId(Guid repositoryId)
        {
            return Ok((await _mediator.Send(
                new GetByRepositoryIdEntitiesCommandRequest(
                    new EntitiesGetByRepositoryIdRequest { RepositoryId = repositoryId }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(EntitiesGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedEntitiesCommandRequest(request))).Message);
        }
    }
}
