using AwesomeGICBank.Application.Dtos;

namespace AwesomeGICBank.Application.Contracts
{
    public interface IInterestService
    {
        Task<InterestRuleDto> CreateInterestRuleAsync(CreateInterestRuleRequest createInterestRuleRequest);

        Task<List<InterestRuleDto>> GetAllRules();
    }
}
