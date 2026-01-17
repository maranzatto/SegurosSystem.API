namespace PropostaService.Application.Interfaces
{
    public interface IDeleteProposalUseCase
    {
        Task Execute(Guid id);
    }
}
