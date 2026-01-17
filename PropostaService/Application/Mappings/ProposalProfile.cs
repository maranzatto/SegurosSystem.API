using AutoMapper;
using PropostaService.Application.DTOs;

namespace PropostaService.Application.Mappings
{
    public class ProposalProfile : Profile
    {
        public ProposalProfile()
        {
            CreateMap<Proposal, ProposalResponseDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => (int)s.Status))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}
