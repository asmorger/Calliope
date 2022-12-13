using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Shouldly;
using Xunit;
using static Calliope.ResultExtensions;

namespace Calliope.Tests;

public class ResultTestsAsync
{
    private Task<Result<int>> SuccessIntAsync(int? value = null) => OkAsync(value ?? 42);
    private Task<Result<int>> FailIntAsync() => FailAsync<int>(new ErrorMessage("error int"));
    private Task<Result<string>> SuccessStringAsync(int number) => OkAsync($"Value {number}");
    private Task<Result<string>> FailureAsync(string? value = null) => 
        FailAsync<string>(new ErrorMessage(value ?? "error"));

    [Fact]
    public async Task Monadic_comprehension_works_when_all_results_are_successful()
    {
        var result = await
            from i in SuccessIntAsync()
            from s in SuccessStringAsync(i)
            select s;

        result.Should().BeSuccessful();
    }

    [Fact]
    public async Task Monadic_comprehension_early_exits_when_first_result_is_failure()
    {
        var result = await
            from i in FailIntAsync()
            from s in SuccessStringAsync(i)
            select s;

        result.Should().BeError();
    }

    [Fact]
    public async Task Monadic_comprehension_early_exits_when_second_result_is_failure()
    {
        var result = await
            from i in SuccessIntAsync()
            from s in FailureAsync()
            select s;

        result.Should().BeError();
    }

    [Fact]
    public async Task Monadic_comprehension_early_exits_on_first_failure()
    {
        var result = await
            from i in FailIntAsync()
            from s in FailureAsync()
            select s;

        result.Should().BeError();
        result.IsError(out var e);
        e!.ToErrorMessage().ShouldBe("error int");
    }


    [Fact]
    public async Task Monadic_comprehension_executes_many_iterations()
    {
        var result = await
            from five in SuccessIntAsync(5)
            from ten in SuccessIntAsync(10)
            from fifteen in SuccessIntAsync(15)
            from s in SuccessIntAsync(five + ten + fifteen)
            select s;

        result.Should().BeSuccessful();
        result.IsOk(out var value);
        value.ShouldBe(30);
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