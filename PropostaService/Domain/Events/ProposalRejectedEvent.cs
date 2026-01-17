namespace PropostaService.Domain.Events
{
    public class ProposalRejectedEvent : IDomainEvent
    {
        public Guid ProposalId { get; }
        public string RejectionReason { get; }
        public DateTime RejectedAt { get; }
        public DateTime OccurredOn { get; }
        public Guid EventId { get; }
        public ProposalRejectedEvent(Guid proposalId, string rejectionReason, DateTime rejectedAt)
        {
            ProposalId = proposalId;
            RejectionReason = rejectionReason;
            RejectedAt = rejectedAt;
            OccurredOn = DateTime.UtcNow;
            EventId = Guid.NewGuid();
        }
    }
}