using System.Collections.Generic;
using FluentAssertions;
using Shouldly;
using Xunit;
using static Calliope.ResultExtensions;

namespace Calliope.Tests;

public class ResultTests
{
    private Result<int> SuccessInt(int? value = null) => Ok(value ?? 42);
    private Result<int> FailInt() => Fail<int>(new ErrorMessage("error int"));
    private Result<string> SuccessString(int number) => Ok($"Value {number}");
    private Result<string> Failure(string? value = null) => Fail<string>(new ErrorMessage(value ?? "error"));

    [Fact]
    public void Monadic_comprehension_works_when_all_results_are_successful()
    {
        var result =
            from i in SuccessInt()
            from s in SuccessString(i)
            select s;

        result.Should().BeSuccessful();
    }

    [Fact]
    public void Monadic_comprehension_early_exits_when_first_result_is_failure()
    {
        var result =
            from i in FailInt()
            from s in SuccessString(i)
            select s;

        result.Should().BeError();
    }

    [Fact]
    public void Monadic_comprehension_early_exits_when_second_result_is_failure()
    {
        var result =
            from i in SuccessInt() 
            from s in Failure()
            select s;

        result.Should().BeError();
    }

    [Fact]
    public void Monadic_comprehension_early_exits_on_first_failure()
    {
        var result =
            from i in FailInt()
            from s in Failure()
            select s;

        result.Should().BeError();
        result.IsError(out var e);
        e!.ToErrorMessage().ShouldBe("error int");
    }


    [Fact]
    public void Monadic_comprehension_executes_many_iterations()
    {
        var result =
            from five in SuccessInt(5)
            from ten in SuccessInt(10)
            from fifteen in SuccessInt(15)
            from s in SuccessInt(five + ten + fifteen)
            select s;

        result.Should().BeSuccessful();
        result.IsOk(out var value);
        value.ShouldBe(30);
    }

    [Fact]
    public void Monadic_comprehension_early_exits_when_where_clause_is_not_matched()
    {
        var result =
            from i in SuccessInt()
            where i == -1
            select i;

        result.Should().BeError();

        result.IsError(out var message);
        message!.ToErrorMessage().Should().EndWith("did not satisfy i => (i == -1)");

    }

    private class ErrorMessage : DomainError
    {
        public ErrorMessage(string message)
        {
            Messages.Add(message);
        }

        public List<string> Messages { get; } = new();
        public string ToErrorMessage() => Messages[0];
    }
}