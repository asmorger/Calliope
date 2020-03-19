using System.Collections.Generic;
using System.Linq;

namespace Calliope
{
    public interface IInternalEventHandler
    {
        void Handle(object @event);
    }
    
    public abstract class AggregateRoot<TId> : IInternalEventHandler
        where TId : ValueObject<TId>
    {
        private readonly List<object> _changes = new List<object>();

        public TId Id { get; protected set; }

        public int Version { get; private set; } = -1;

        void IInternalEventHandler.Handle(object @event) => When(@event);

        protected abstract void When(object @event);

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }

        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

        public void Load(IEnumerable<object> history)
        {
            foreach (var e in history)
            {
                When(e);
                Version++;
            }
        }

        public void ClearChanges() => _changes.Clear();

        protected abstract void EnsureValidState();

        protected void ApplyToEntity(IInternalEventHandler entity, object @event) => entity?.Handle(@event);
    }
}