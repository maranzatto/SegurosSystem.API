namespace ContratacaoService.Application.Interfaces
{
    public interface IContractPolicyUseCase
    {
        Task<Guid> ExecuteAsync(Guid proposalId);
    }
}
