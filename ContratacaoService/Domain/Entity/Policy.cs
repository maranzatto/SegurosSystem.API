using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.Common;
using ContratacaoService.Domain.Events;
using ContratacaoService.Domain.Exceptions;
using System;
using System.Runtime.InteropServices;
using ContratacaoService.Domain.ValueObjects;

namespace ContratacaoService.Domain.Entities
{
    public class Policy : IEntity
    {
        private readonly IClock _clock;

        public Guid Id { get; private set; }
        public Guid ProposalId { get; private set; }
        public PolicyNumber PolicyNumber { get; private set; }
        public PolicyPeriod Period { get; private set; }
        public DateTime ContractedAt { get; private set; }
        public PolicyStatus Status { get; private set; }
        public bool IsDeleted { get; private set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        private Policy()
        {
            _clock = null!;
            IsDeleted = false;
        }

        private Policy(Guid id, Guid proposalId, IClock clock)
        {
            _clock = clock;
            Id = id;
            ProposalId = proposalId;
            PolicyNumber = PolicyNumber.Generate();
            ContractedAt = _clock.UtcNow;
            Period = new PolicyPeriod(ContractedAt);
            Status = PolicyStatus.Active;
            IsDeleted = false;
        }

        public static Policy Create(Guid proposalId, ProposalStatusContract proposalStatus, IClock clock)
        {
            if (proposalStatus != ProposalStatusContract.Approved)
                throw new DomainException("Proposta não aprovada não pode ser contratada.");

            return new Policy(Guid.NewGuid(), proposalId, clock);
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