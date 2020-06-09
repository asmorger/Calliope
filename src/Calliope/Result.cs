using System;

namespace Calliope
{
    public class Result<TSuccess> : Either<TSuccess, DomainException>
        where TSuccess : notnull
    {
        private Result(TSuccess success) : base(success)
        {
        }

        private Result(DomainException domainException) : base(domainException)
        {
        }

        public T HandleSuccessOrError<T>(Func<TSuccess, T> successHandler, Func<DomainException, T> errorHandler) =>
            Match(successHandler, errorHandler);

        public void HandleError(Action<DomainException> domainExceptionAction) => DoRight(domainExceptionAction);

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

        public bool IsError(out DomainException domainException)
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
        public static Result<TSuccess> Error(DomainException domainException) => new Result<TSuccess>(domainException);
    }
}