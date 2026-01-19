namespace ContratacaoService.Application.Interfaces
{
    public interface IDeletePolicyUseCase
    {
        Task Execute(Guid id);
    }
}
