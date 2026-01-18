namespace ContratacaoService.Domain.Events
{
    public interface IEntity
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void AddDomainEvent(IDomainEvent domainEvent);
        void ClearDomainEvents();
    }
}
