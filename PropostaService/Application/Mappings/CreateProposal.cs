using AutoMapper;
using PropostaService.Application.DTOs;

namespace PropostaService.Application.Mappings
{
    public class CreateProposal : Profile
    {
        public CreateProposal()
        {
            CreateMap<CreateProposalRequestDto, Proposal>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));
                
        }
    }
}
