namespace ContratacaoService.Application.Interfaces
{
    public interface IRestorePolicyUseCase
    {
        Task Execute(Guid id);
    }
}
