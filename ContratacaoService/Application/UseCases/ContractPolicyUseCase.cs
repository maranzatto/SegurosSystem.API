using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Interfaces.Repositories;
using ContratacaoService.Domain.Common;
using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Application.UseCases
{
    public class ContractPolicyUseCase : IContractPolicyUseCase
    {
        private readonly IProposalHttpClient _proposalClient;
        private readonly IPolicyRepository _policyRepository;
        private readonly IClock _clock;

        public ContractPolicyUseCase(IProposalHttpClient proposalClient, IPolicyRepository policyRepository, IClock clock)
        {
            _proposalClient = proposalClient ?? throw new ArgumentNullException(nameof(proposalClient));
            _policyRepository = policyRepository ?? throw new ArgumentNullException(nameof(policyRepository));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public async Task<Guid> ExecuteAsync(Guid proposalId)
        {
            var proposal = await _proposalClient.GetByIdAsync(proposalId);

            var policy = Policy.Create(
                proposalId,
                proposal.Status,
                proposal.Description,
                _clock);

            await _policyRepository.AddAsync(policy);

            return policy.Id;
        }
    }
}
