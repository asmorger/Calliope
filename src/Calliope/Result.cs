 #nullable enable
 using System;
 using System.Diagnostics.CodeAnalysis;

 namespace Calliope
{
    public record Result<T>
    {
        public record Success(T Value) : Result<T>;

        public record Failure(DomainError Error) : Result<T>;
        
        private Result() { }

        public TOutput Match<TOutput>(Func<T, TOutput> onSuccess,
            Func<DomainError, TOutput> onFailure) =>
            this switch
            {
                Failure failure => onFailure(failure.Error),
                Success success => onSuccess(success.Value),
                _ => throw new ArgumentOutOfRangeException()
            };

        public bool IsError([NotNullWhen(true)] out DomainError? error)
        {
            if (this is Failure failure)
            {
                error = failure.Error;
                return true;
            }

            error = null;
            return false;
        }

        public bool IsError() => this is Failure;
        public bool IsOk() => this is Success;
    }
}