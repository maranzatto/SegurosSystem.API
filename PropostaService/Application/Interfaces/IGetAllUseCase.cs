using PropostaService.Application.DTOs;

namespace PropostaService.Application.Interfaces
{
    public interface IGetAllUseCase
    {
        Task<IEnumerable<ProposalResponseDto>> Execute();
    }
}
