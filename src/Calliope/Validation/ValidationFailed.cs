using System.Collections.Generic;
using System.Text;

namespace Calliope.Validation
{
    public class ValidationFailed : DomainError
    {
        public ValidationFailed(IEnumerable<string> validationMessages) =>
            Messages.AddRange(validationMessages);

        public List<string> Messages { get; } = new List<string>();

        public string ToErrorMessage()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Validation Failed!");

            foreach(var message in Messages)
            {
                builder.Append("  ");
                builder.AppendLine(message);
            }

            return builder.ToString();
        }
    }
}