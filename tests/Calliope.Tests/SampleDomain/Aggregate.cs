using Calliope.Validators;

namespace Calliope.Tests.SampleDomain
{
    public class AggregateId : ValueObject<int, AggregateId, PositiveIntegerValidator> {
        public AggregateId(int source) : base(source) { }
    }
    
    public class Aggregate : AggregateRoot<AggregateId>
    {
        protected override void When(object @event)
        {
            
        }

        protected override void EnsureValidState()
        {
            
        }
    }
}