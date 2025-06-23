namespace Domain.Events;

public abstract class MemberChangedEvents : DomainEvents
{
    protected EntityChangedEvent EntityChangedDomainEvent { get; }

    protected MemberChangedEvents(EntityChangedEvent entityModifiedDomainEvent)
    {
        EntityChangedDomainEvent = entityModifiedDomainEvent;
    }

    public virtual void Add<T>(CollectionChangedEvent<T> change)
    {
        EntityChangedDomainEvent.Add(change);
        Add(EntityChangedDomainEvent);
    }
}
