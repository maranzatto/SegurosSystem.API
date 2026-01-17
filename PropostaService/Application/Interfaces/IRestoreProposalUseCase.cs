namespace PropostaService.Application.Interfaces
{
    public interface IRestoreProposalUseCase
    {
        Task Execute(Guid id);
    }
}
