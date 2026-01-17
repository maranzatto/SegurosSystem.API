using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;
using PropostaService.Domain.Exceptions;
using PropostaService.Infrastructure.Repositories;

namespace PropostaService.Application.UseCases
{
    public class RejectProposalUseCase : IRejectProposalUseCase
    {
        private readonly IProposalRepository _repository;

        public RejectProposalUseCase(IProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(Guid id, string reason)
        {
            var proposal = await _repository.GetByIdAsync(id)
        ?? throw new DomainException("Proposta não encontrada.");

            proposal.Reject(reason);
            await _repository.UpdateAsync(proposal);
        }
    }
}
