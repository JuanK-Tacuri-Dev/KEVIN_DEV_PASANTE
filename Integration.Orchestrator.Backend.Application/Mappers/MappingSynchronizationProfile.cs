using AutoMapper;
using Integration.Orchestrator.Backend.Application.Models.Configurator.Synchronization;
using Integration.Orchestrator.Backend.Application.Models.Configurator.SynchronizationStatus;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;

namespace Integration.Orchestrator.Backend.Application.Mapping
{
    public class MappingSynchronizationProfile : Profile
    {

        public MappingSynchronizationProfile()
        {
            CreateMap<SynchronizationResponseModel, SynchronizationGetAllPaginated>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.synchronization_code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.synchronization_name))
                .ForMember(dest => dest.FranchiseId, opt => opt.MapFrom(src => src.franchise_id))
                .ForMember(dest => dest.HourToExecute, opt => opt.MapFrom(src => src.synchronization_hour_to_execute))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.user_id))
                .ForMember(dest => dest.Observations, opt => opt.MapFrom(src => src.synchronization_observations))
                .ForMember(dest => dest.Integrations, opt => opt.MapFrom(src => src.integrations.Select(i => new IntegrationResponse { Id = i }).ToList()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.SynchronizationStates.FirstOrDefault()));

          //  CreateMap<SynchronizationStateResponseModel, SynchronizationStatusResponse>()
          //      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          //      .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.synchronization_status_key))
          //      .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.synchronization_status_text))
          //      .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.synchronization_status_color))
          //      .ForMember(dest => dest.created, opt => opt.MapFrom(src => src.created_at))
          //      .ForMember(dest => dest.updated, opt => opt.MapFrom(src => src.updated_at))
          //      .ForMember(dest => dest.Background, opt => opt.MapFrom(src => src.synchronization_status_background));
        }


    }
}
