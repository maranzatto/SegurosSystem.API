using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;
using PropostaService.Domain;
using PropostaService.Domain.Common;
using PropostaService.Infrastructure.Repositories;

namespace PropostaService.Application.UseCases
{
    public class CreateProposalUseCase : ICreateProposalUseCase
    {
        private readonly IProposalRepository _repository;
        private readonly IClock _clock;

        public CreateProposalUseCase(IProposalRepository repository, IClock clock)
        {
            _repository = repository;
            _clock = clock;
        }

        public async Task<Guid> Execute(CreateProposalRequestDto request)
        {
            var proposal = new Proposal(
                Guid.NewGuid(),
                request.Description,
                _clock
            );

            await _repository.AddAsync(proposal);

            return proposal.Id;
        }
    }
}
