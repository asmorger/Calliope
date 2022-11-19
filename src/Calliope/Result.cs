using System;

namespace Calliope
{
    public class Result<TSuccess> : Either<TSuccess, DomainError>
        where TSuccess : notnull
    {
        private Result(TSuccess success) : base(success)
        {
        }

        private Result(DomainError domainException) : base(domainException)
        {
        }

        public T HandleSuccessOrError<T>(Func<TSuccess, T> successHandler, Func<DomainError, T> errorHandler) =>
            Match(successHandler, errorHandler);

        public void HandleError(Action<DomainError> domainExceptionAction) => DoRight(domainExceptionAction);

        public bool IsOk() => base.MatchLeft(x => x).IsSome();
        public bool IsError() => base.MatchRight(x => x).IsSome();

        public bool IsOk(out TSuccess result)
        {
            var source = base.MatchLeft(x => x);

            if (source.IsSome(out var sourceOk))
            {
                result = sourceOk;
                return true;
            }

            result = default!;
            return false;
        }

        public bool IsError(out DomainError domainException)
        {
            var source = base.MatchRight(x => x);

            if (source.IsSome(out var sourceException))
            {
                domainException = sourceException;
                return true;
            }

            domainException = null;
            return false;
        }

        public TSuccess Unwrap() => base.MatchLeft(x => x).Unwrap();

        public static Result<TSuccess> Ok(TSuccess successValue) => new Result<TSuccess>(successValue);
        public static Result<TSuccess> Error(DomainError domainException) => new Result<TSuccess>(domainException);
    }
}