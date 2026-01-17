using PropostaService.Application.DTOs;

namespace PropostaService.Application.Interfaces
{
    public interface ICreateProposalUseCase
    {
        Task<Guid> Execute(CreateProposalRequestDto dto);
    }
}
