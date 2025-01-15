using Integration.Orchestrator.Backend.Application.Models.Configurator.Transformation;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Handlers.Configurator.Transformation
{
    [ExcludeFromCodeCoverage]
    public class TransformationCommands
    {
        public readonly record struct GetAllPaginatedTransformationCommandRequest(TransformationGetAllPaginatedRequest Transformation) 
            : IRequest<GetAllPaginatedTransformationCommandResponse>;
        public readonly record struct GetAllPaginatedTransformationCommandResponse(TransformationGetAllPaginatedResponse Message);
    }
}
