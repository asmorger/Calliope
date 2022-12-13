using Calliope;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

// to reduce the namespace conflicts, put this in the FluentValidations namespace #fun-hack
// ReSharper disable once CheckNamespace
namespace FluentAssertions;

public static class ResultExtensions
{
    public static ResultAssertions<TSuccess> Should<TSuccess>(this Result<TSuccess> result) where TSuccess : notnull =>
        new(result);
}

public class ResultAssertions<TSuccess> : ReferenceTypeAssertions<Result<TSuccess>, ResultAssertions<TSuccess>> where TSuccess : notnull
{
    public ResultAssertions(Result<TSuccess> instance) : base(instance) { }
    protected override string Identifier => "result";

    public AndConstraint<ResultAssertions<TSuccess>> BeSuccessful()
    {
        Execute.Assertion
            .Given(() => Subject.IsOk())
            .ForCondition(isOk => isOk)
            .FailWith(BuildErrorDisplayMessage());

        return new AndConstraint<ResultAssertions<TSuccess>>(this);
    }

    public AndConstraint<ResultAssertions<TSuccess>> BeError()
    {
        Execute.Assertion
            .Given(() => Subject.IsOk())
            .ForCondition(isOk => !isOk)
            .FailWith(BuildErrorDisplayMessage());

        return new AndConstraint<ResultAssertions<TSuccess>>(this);
    }

    private string BuildErrorDisplayMessage()
    {
        if(Subject.IsError(out var error))
        {
            return error.ToErrorMessage();
        }

        return string.Empty;
    }
}