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
}

public static class ResultExtensions
{
    public static Result<T> Ok<T>(T value) => new Result<T>.Success(value);
    public static Task<Result<T>> OkAsync<T>(T value) =>
        Task.FromResult( (Result<T>) new Result<T>.Success(value));
    public static Result<T> Fail<T>(DomainError error) => new Result<T>.Failure(error);
    public static Task<Result<T>> FailAsync<T>(DomainError error) => 
        Task.FromResult( (Result<T>) new Result<T>.Failure(error));
   
    public static Result<TResult> SelectMany<TFirst, TSecond, TResult>(
        this Result<TFirst> first,
        Func<TFirst, Result<TSecond>> getSecond,
        Func<TFirst, TSecond, TResult> getResult)
    {
        return first.Match(
            // First operand has value -> continue to the second operand
            firstValue => getSecond(firstValue).Match(
                // Second operand has value -> compose the result from the first and second operands
                secondValue => Ok(getResult(firstValue, secondValue)),
    
                // Second operand is error -> return
                Fail<TResult> 
            ),
    
            // First operand is error -> return
            Fail<TResult> 
        );
    }
    
    public static async Task<Result<TResult>> SelectMany<TFirst, TSecond, TResult>(
        this Task<Result<TFirst>> first,
        Func<TFirst, Task<Result<TSecond>>> getSecond,
        Func<TFirst, TSecond, TResult> getResult)
    {
        var firstOption = await first;
    
        return await firstOption.Match(
            async firstValue =>
            {
                var secondOption = await getSecond(firstValue);
    
                return secondOption.Match(
                    secondValue => Ok(getResult(firstValue, secondValue)),
                    Fail<TResult>
                );
            },
            e => Task.FromResult(Fail<TResult>(e))
        );
    }
}