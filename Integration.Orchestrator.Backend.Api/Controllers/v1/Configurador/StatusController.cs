﻿using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Status;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Status.StatusCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurador
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/states/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class StatusController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(StatusCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateStatusCommandRequest(
                    new StatusBasicInfoRequest<StatusCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(StatusUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(
                new UpdateStatusCommandRequest(
                    new StatusBasicInfoRequest<StatusUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeleteStatusCommandRequest(
                    new StatusDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdStatusCommandRequest(
                    new StatusGetByIdRequest { Id = id }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(StatusGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedStatusCommandRequest(request))).Message);
        }
    }
}
