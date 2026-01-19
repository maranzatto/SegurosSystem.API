using ContratacaoService.Application.DTOs;

namespace ContratacaoService.Application.Interfaces
{
    public interface IGetAllUseCase
    {
        Task<IEnumerable<PolicyResponseDto>> Execute();
    }
}
