using System.Collections.Generic;

namespace Calliope
{
    // https://enterprisecraftsmanship.com/posts/new-online-course-ddd-and-ef-core/
    // implementation based upon Vladimir Khorikov's solution
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}