using ContratacaoService.Domain.Enums;

namespace ContratacaoService.Application.DTOs
{
    public class ProposalDto
    {
        public Guid Id { get; set; }
        public ProposalStatusContract Status { get; set; }
        public string StatusName { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

}
