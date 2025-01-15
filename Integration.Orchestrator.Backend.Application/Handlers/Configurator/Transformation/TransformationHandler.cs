using AutoMapper;
using Integration.Orchestrator.Backend.Application.Models.Configurator.Catalog;
using Integration.Orchestrator.Backend.Application.Models.Configurator.Connection;
using Integration.Orchestrator.Backend.Application.Models.Configurator.Transformation;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Mapster;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Catalog.CatalogCommands;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Connection.ConnectionCommands;
using static Integration.Orchestrator.Backend.Application.Handlers.Configurator.Transformation.TransformationCommands;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Transformation
{
    [ExcludeFromCodeCoverage]
    public class TransformationHandler(ITransformationService<TransformationEntity> TransformationService, IMapper mapper)
        : IRequestHandler<GetAllPaginatedTransformationCommandRequest, GetAllPaginatedTransformationCommandResponse>

    {
        public readonly ITransformationService<TransformationEntity> _transformationService = TransformationService;
        public readonly IMapper _mapper = mapper;
        public async Task<GetAllPaginatedTransformationCommandResponse> Handle(GetAllPaginatedTransformationCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Transformation.Adapt<PaginatedModel>();
                var rows = await _transformationService.GetTotalRowsAsync(model);
                if (rows == 0)
                {
                    return new GetAllPaginatedTransformationCommandResponse(
                    new TransformationGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = new TransformationGetAllRows
                        {
                            Total_rows = rows,
                            Rows = []
                        }
                    });
                }
                var data = await _transformationService.GetAllAsync(model);

                return new GetAllPaginatedTransformationCommandResponse(
                    new TransformationGetAllPaginatedResponse
                    {
                        Code = (int)ResponseCode.FoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.FoundSuccessfully),
                        Data = new TransformationGetAllRows
                        {
                            Total_rows = rows,
                            Rows = data.Select(model => new TransformationGetAllPaginated
                            {
                                Id = model.id,
                                code= model.transformation_code,
                                name= model.transformation_name,
                                description= model.description

                            }).ToList()
                        }
                    });
            }
            catch (OrchestratorArgumentException ex)
            {
                throw new OrchestratorArgumentException(string.Empty, ex.Details);
            }
            catch (Exception ex)
            {
                throw new OrchestratorException(ex.Message);
            }
        }
    }
}
