using Integration.Orchestrator.Backend.Api.Filter;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurador.Catalog.CatalogCommands;

namespace Integration.Orchestrator.Backend.Api.Controllers.v1.Configurador
{
    [ExcludeFromCodeCoverage]
    [Route("api/v1/catalogs/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ErrorHandlingRest))]
    public class CatalogController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CatalogCreateRequest request)
        {
            return Ok((await _mediator.Send(
                new CreateCatalogCommandRequest(
                    new CatalogBasicInfoRequest<CatalogCreateRequest>(request)))).Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CatalogUpdateRequest request, Guid id)
        {
            return Ok((await _mediator.Send(
                new UpdateCatalogCommandRequest(
                    new CatalogBasicInfoRequest<CatalogUpdateRequest>(request), id))).Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok((await _mediator.Send(
                new DeleteCatalogCommandRequest(
                    new CatalogDeleteRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok((await _mediator.Send(
                new GetByIdCatalogCommandRequest(
                    new CatalogGetByIdRequest { Id = id }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFatherCode(int code)
        {
            return Ok((await _mediator.Send(
                new GetByFatherCatalogCommandRequest(
                    new CatalogGetByFatherRequest { FatherCode = code }))).Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(int code)
        {
            return Ok((await _mediator.Send(
                new GetByCodeCatalogCommandRequest(
                    new CatalogGetByCodeRequest { Code = code }))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginated(CatalogGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedCatalogCommandRequest(request))).Message);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPaginatedCarlos(CatalogGetAllPaginatedRequest request)
        {
            return Ok((await _mediator.Send(
                new GetAllPaginatedCatalogCommandRequest(request))).Message);
        }
    }
}
