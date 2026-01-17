using PropostaService.Application.DTOs;

namespace PropostaService.Application.Interfaces
{
    public interface IRejectProposalUseCase
    {
        Task Execute(Guid id, string reason);
    }
}
