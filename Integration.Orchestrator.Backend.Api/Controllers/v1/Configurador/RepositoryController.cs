﻿using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Repository.RepositoryCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurador
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/repositories/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class RepositoryController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(RepositoryCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateRepositoryCommandRequest(
                    new RepositoryBasicInfoRequest<RepositoryCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(RepositoryUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(
                new UpdateRepositoryCommandRequest(
                    new RepositoryBasicInfoRequest<RepositoryUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeleteRepositoryCommandRequest(
                    new RepositoryDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdRepositoryCommandRequest(
                    new RepositoryGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok((await _mediator.Send(
                new GetByCodeRepositoryCommandRequest(
                    new RepositoryGetByCodeRequest { Code = code }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(RepositoryGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedRepositoryCommandRequest(request))).Message);
        }
    }
}
