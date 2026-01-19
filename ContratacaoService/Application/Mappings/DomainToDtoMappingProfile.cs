using AutoMapper;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Enums;

namespace ContratacaoService.Application.Mappings
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Policy, PolicyResponseDto>()
                .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.Period.Start))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.Period.End))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.PolicyNumber.Value));
        }
    }
}
