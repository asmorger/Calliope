using System.Collections.Generic;

namespace Calliope;

// https://enterprisecraftsmanship.com/posts/new-online-course-ddd-and-ef-core/
// implementation based upon Vladimir Khorikov's solution
public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}