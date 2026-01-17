namespace PropostaService.Application.Interfaces
{
    public interface IApproveProposalUseCase
    {
        Task Execute(Guid id);
    }
}
