using AutoMapper;
using Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Catalog;

namespace Integration.Orchestrator.Backend.Application.Mappers
{
    public class MappingCatalogProfile : Profile
    {
        public MappingCatalogProfile()
        {
            CreateMap<CatalogResponseModel, CatalogGetAllPaginated>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.catalog_code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.catalog_name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.catalog_value))
                .ForMember(dest => dest.FatherCode, opt => opt.MapFrom(src => src.father_code))
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.catalog_detail))
                .ForMember(dest => dest.IsFather, opt => opt.MapFrom(src => src.is_father))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.status_id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.FirstOrDefault()));

            CreateMap<StatusResponseModel, StatusResponse>()
                .ForMember(dest => dest.key, opt => opt.MapFrom(src => src.status_key))
                .ForMember(dest => dest.text, opt => opt.MapFrom(src => src.status_text))
                .ForMember(dest => dest.color, opt => opt.MapFrom(src => src.status_color))
                .ForMember(dest => dest.background, opt => opt.MapFrom(src => src.status_background))
                .ForMember(dest => dest.created, opt => opt.MapFrom(src => src.created_at))
                .ForMember(dest => dest.updated, opt => opt.MapFrom(src => src.updated_at))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }

}
