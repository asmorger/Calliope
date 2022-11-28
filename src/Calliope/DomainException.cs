using System.Collections.Generic;

namespace Calliope;

public interface DomainError
{
    public List<string> Messages { get; }

    public string ToErrorMessage();
}