using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.Common;
using ContratacaoService.Domain.Exceptions;
using ContratacaoService.Domain.ValueObjects;

namespace ContratacaoService.Domain.Entities
{
    public class Policy
    {
        private readonly IClock _clock;

        public Guid Id { get; private set; }
        public Guid ProposalId { get; private set; }
        public PolicyNumber PolicyNumber { get; private set; }
        public PolicyPeriod Period { get; private set; }
        public string ProposalName { get; set; }
        public DateTime ContractedAt { get; private set; }
        public PolicyStatus Status { get; private set; }
        public bool IsDeleted { get; private set; }

        private Policy()
        {
            _clock = null!;
            PolicyNumber = null!;
            ProposalName = null!;
            Period = null!;
            IsDeleted = false;
        }

        private Policy(Guid id, Guid proposalId, string proposalName, IClock clock)
        {
            _clock = clock;
            Id = id;
            ProposalId = proposalId;
            PolicyNumber = PolicyNumber.Generate();
            ProposalName = proposalName;
            ContractedAt = _clock.UtcNow;
            Period = new PolicyPeriod(ContractedAt);
            Status = PolicyStatus.Active;
            IsDeleted = false;
        }

        public static Policy Create(Guid proposalId, ProposalStatusContract proposalStatus, string proposalName, IClock clock)
        {
            if (proposalStatus != ProposalStatusContract.Approved)
                throw new DomainException("Proposta não aprovada não pode ser contratada.");

            return new Policy(Guid.NewGuid(), proposalId, proposalName, clock);
        }

        public void Cancel()
        {
            if (Status != PolicyStatus.Active)
                throw new DomainException("Apenas apólices ativas podem ser canceladas.");

            Status = PolicyStatus.Canceled;
        }

        public void Delete()
        {
            if (IsDeleted)
                throw new DomainException("Apólice já excluída.");

            IsDeleted = true;
        }

        public void Restore()
        {
            if (!IsDeleted)
                throw new DomainException("Apólice não está excluída.");

            IsDeleted = false;
        }

    }
}