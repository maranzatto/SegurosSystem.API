using PropostaService.Application.Interfaces;
using PropostaService.Domain.Exceptions;
using PropostaService.Infrastructure.Repositories;

namespace PropostaService.Application.UseCases
{
    public class DeleteProposalUseCase : IDeleteProposalUseCase
    {
        private readonly IProposalRepository _repository;

        public DeleteProposalUseCase(IProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(Guid id)
        {
            var proposal = await _repository.GetByIdAsync(id)
                ?? throw new DomainException("Proposta não encontrada.");

            proposal.Delete();

            await _repository.UpdateAsync(proposal);
        }
    }
}
