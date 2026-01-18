namespace ContratacaoService.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
        Guid EventId { get; }
    }
}