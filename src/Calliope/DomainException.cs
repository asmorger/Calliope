using System.Collections.Generic;

namespace Calliope;

public interface DomainError
{
    public List<string> Messages { get; }

    public string ToErrorMessage();
}

public record Message(string Value) : DomainError
{
    public List<string> Messages => new(new[] {Value});
    public string ToErrorMessage() => Value;
}