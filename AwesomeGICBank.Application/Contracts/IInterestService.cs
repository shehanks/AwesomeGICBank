using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Application.Contracts
{
    public interface IInterestService
    {
        Task CreateInterestRuleAsync(InterestRule interestRule);
    }
}
