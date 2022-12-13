using MediatR;
using static Calliope.ResultExtensions;

namespace Calliope.SampleDomain.Accounts.Usecases;

public class FindAccountBalancyById : IRequest<Result<decimal>>
{
    
}

public class FindAccountByIdHandler : IRequestHandler<FindAccountBalancyById, Result<decimal>>
{
    public Task<Result<decimal>> Handle(FindAccountBalancyById request, CancellationToken cancellationToken)
    {
        var accountBalance =
            from person in PersonRepository.GetPerson(0)
            from account in BankAccountRepository.GetAccount(0)
            from value in GetAccountBalance(account, person)
            select value;

        return Task.FromResult(accountBalance);
    }

    private Result<decimal> GetAccountBalance(BankAccount account) => Ok(25m);
    private Result<decimal> GetAccountBalance(BankAccount account, Person person) => Ok(25m);
}