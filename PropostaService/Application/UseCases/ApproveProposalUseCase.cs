using PropostaService.Application.Interfaces;
using PropostaService.Domain.Exceptions;
using PropostaService.Infrastructure.Repositories;

namespace PropostaService.Application.UseCases
{
    public class ApproveProposalUseCase : IApproveProposalUseCase
    {
        private readonly IProposalRepository _repository;

        public ApproveProposalUseCase(IProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(Guid id)
        {
            var proposal = await _repository.GetByIdAsync(id)
                ?? throw new DomainException("Proposta não encontrada.");

            proposal.Approve();

            await _repository.UpdateAsync(proposal);
        }
    }

}
