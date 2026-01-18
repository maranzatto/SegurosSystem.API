using ContratacaoService.Application.DTOs;

namespace ContratacaoService.Application.Interfaces.Repositories
{
    public interface IProposalHttpClient
    {
        Task<ProposalDto> GetByIdAsync(Guid proposalId);
    }
}
