using PropostaService.Application.DTOs;

namespace PropostaService.Application.Interfaces
{
    public interface IGetProposalByIdUseCase
    {
        Task<ProposalResponseDto> Execute(Guid id);
    }

}
