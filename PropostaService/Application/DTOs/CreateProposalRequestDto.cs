using PropostaService.Domain.ValueObjects;

namespace PropostaService.Application.DTOs
{
    public class CreateProposalRequestDto
    {
        public ProposalDescription Description { get; set; }
    }
}
