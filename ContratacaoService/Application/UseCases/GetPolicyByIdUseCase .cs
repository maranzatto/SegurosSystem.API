using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Interfaces.Repositories;

namespace ContratacaoService.Application.UseCases
{
    public class GetPolicyByIdUseCase : IGetPolicyByIdUseCase
    {
        private readonly IPolicyRepository _policyRepository;

        public GetPolicyByIdUseCase(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository ?? throw new ArgumentNullException(nameof(policyRepository));
        }

        public async Task<PolicyResponseDto> ExecuteAsync(Guid id)
        {
            var policy = await _policyRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Apólice não encontrada.");

            return new PolicyResponseDto
            {
                Id = policy.Id,
                ProposalId = policy.ProposalId,
                PolicyNumber = policy.PolicyNumber.Value,
                ProposalName = policy.ProposalName,
                ContractedAt = policy.ContractedAt,
                EffectiveDate = policy.Period.Start,
                ExpirationDate = policy.Period.End,
                Status = policy.Status.ToString()
            };
        }
    }
}
