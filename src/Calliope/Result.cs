#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Calliope; 

public record Result<T>
{
    private Result() { }
    
    public record Success(T Value) : Result<T>;

    public record Failure(DomainError Error) : Result<T>;

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
    
    public bool IsOk([NotNullWhen(true)] out T? value)
    {
        if (this is Success success)
        {
            value = success.Value!;
            return true;
        }

        value = default;
        return false;
    }

    public bool IsError() => this is Failure;
    public bool IsOk() => this is Success;

    public Result<TOutput> Select<TOutput>(Func<T, Result<TOutput>> onSuccess) =>
        Match(onSuccess.Invoke, e => new Result<TOutput>.Failure(e));
    
    public async Task<Result<TOutput>> SelectAsync<TOutput>(Func<T, Task<Result<TOutput>>> onSuccess) =>
        this switch
        {
            Success success => await onSuccess.Invoke(success.Value),
            Failure failure => new Result<TOutput>.Failure(failure.Error),
            _ => throw new ArgumentOutOfRangeException()
        };
}

public static class ResultExtensions
{
    public static Result<T> Ok<T>(T value) => new Result<T>.Success(value);
    public static Result<T> Fail<T>(DomainError error) => new Result<T>.Failure(error);
    
    public static async Task<Result<TOutput>> SelectAsync<T, TOutput>(this Task<Result<T>> action, 
        Func<T, Task<Result<TOutput>>> onSuccess)
    {
        var result = await action;
        return result switch
        {
            Result<T>.Success success => await onSuccess.Invoke(success.Value),
            Result<T>.Failure failure => new Result<TOutput>.Failure(failure.Error),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static async Task<Result<(T, TOutput)>> SelectAndContinueAsync<T, TOutput>(this Task<Result<T>> action, 
        Func<T, Task<Result<TOutput>>> onSuccess)
    {
        var result = await action;

        switch (result)
        {
            case Result<T>.Success s:
                var result2 = await onSuccess.Invoke(s.Value);
                return result2 switch
                {
                    Result<TOutput>.Success success => new Result<(T, TOutput)>.Success((s.Value, success.Value)),
                    Result<TOutput>.Failure failure => new Result<(T, TOutput)>.Failure(failure.Error),
                    _ => throw new ArgumentOutOfRangeException()
                };
            case Result<T>.Failure f:
                return new Result<(T, TOutput)>.Failure(f.Error);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static async Task<Result<(T, TOutput)>> SelectAndContinueAsync<T, TOutput>(this Task<Result<T>> action, 
        Func<T, Result<TOutput>> onSuccess)
    {
        var result = await action;

        switch (result)
        {
            case Result<T>.Success s:
                var result2 = onSuccess.Invoke(s.Value);
                return result2 switch
                {
                    Result<TOutput>.Success success => new Result<(T, TOutput)>.Success((s.Value, success.Value)),
                    Result<TOutput>.Failure failure => new Result<(T, TOutput)>.Failure(failure.Error),
                    _ => throw new ArgumentOutOfRangeException()
                };
            case Result<T>.Failure f:
                return new Result<(T, TOutput)>.Failure(f.Error);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}