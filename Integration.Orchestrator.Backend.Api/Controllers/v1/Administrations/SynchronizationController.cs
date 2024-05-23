﻿using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administrations.AdministrationsCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administrations
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class SynchronizationController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(SynchronizationCreateRequest request)
        {
            return Ok(await _mediator.Send(new CreateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationCreateRequest>(request))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SynchronizationUpdateRequest request, Guid id)
        {
            return Ok(await _mediator.Send(new UpdateSynchronizationCommandRequest(new SynchronizationBasicInfoRequest<SynchronizationUpdateRequest>(request), id)));
        }

        [HttpGet]
        public async Task<IActionResult> GetByFranchiseId(Guid franchiseId)
        {
            return Ok(await _mediator.Send(new GetByFranchiseIdSynchronizationCommandRequest(new GetByFranchiseIdSynchronizationRequest { FranchiseId = franchiseId })));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated(SynchronizationGetAllPaginatedRequest request)
        {
            return Ok(await _mediator.Send(new GetAllPaginatedSynchronizationCommandRequest(request)));
        }
    }
}
