using ContratacaoService.Application.DTOs;

namespace ContratacaoService.Application.Interfaces
{
    public interface IGetPolicyByIdUseCase
    {
        Task<PolicyResponseDto> ExecuteAsync(Guid id);
    }
}
