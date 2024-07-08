﻿using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Adapter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Integration.Orchestrator.Backend.Application.Handlers.Administration.Adapter.AdapterCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Administration
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class AdaptersController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(AdapterCreateRequest request)
        {
            return Ok(await _mediator.Send(
                new CreateAdapterCommandRequest(
                    new AdapterBasicInfoRequest<AdapterCreateRequest>(request))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(AdapterUpdateRequest request, Guid id)
        {
            return Ok(await _mediator.Send(new UpdateAdapterCommandRequest(
                new AdapterBasicInfoRequest<AdapterUpdateRequest>(request), id)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteAdapterCommandRequest(
                new AdapterDeleteRequest { Id = id })));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _mediator.Send(
                new GetByIdAdapterCommandRequest(
                    new AdapterGetByIdRequest { Id = id })));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Ok(await _mediator.Send(
                new GetByCodeAdapterCommandRequest(
                    new AdapterGetByCodeRequest { Code = code })));
        }
        [HttpGet]
        public async Task<IActionResult> GetByType(string type)
        {
            return Ok(await _mediator.Send(
                new GetByTypeAdapterCommandRequest(
                    new AdapterGetByTypeRequest { Type = type })));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(AdapterGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedAdapterCommandRequest(request))).Message);
        }
    }
}
