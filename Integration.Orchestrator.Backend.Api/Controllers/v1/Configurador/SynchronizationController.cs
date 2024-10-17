using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Synchronization.SynchronizationCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurador
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/Synchronizations/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class SynchronizationController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(SynchronizationCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateSynchronizationCommandRequest(
                    new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(SynchronizationUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(
                new UpdateSynchronizationCommandRequest(
                    new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeleteSynchronizationCommandRequest(
                    new SynchronizationDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdSynchronizationCommandRequest(
                    new SynchronizationGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFranchiseId(Guid franchiseId)
        {
            return Ok((await _mediator.Send(
                new GetByFranchiseIdSynchronizationCommandRequest(
                    new SynchronizationGetByFranchiseIdRequest { FranchiseId = franchiseId }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(SynchronizationGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedSynchronizationCommandRequest(request))).Message);
        }

    }
}
