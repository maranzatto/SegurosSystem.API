namespace PropostaService.Domain.Events
{
    public class ProposalApprovedEvent : IDomainEvent
    {
        public Guid ProposalId { get; }
        public DateTime ApprovedAt { get; }
        public DateTime OccurredOn { get; }
        public Guid EventId { get; }
        public ProposalApprovedEvent(Guid proposalId, DateTime approvedAt)
        {
            ProposalId = proposalId;
            ApprovedAt = approvedAt;
            OccurredOn = DateTime.UtcNow;
            EventId = Guid.NewGuid();
        }
    }
}