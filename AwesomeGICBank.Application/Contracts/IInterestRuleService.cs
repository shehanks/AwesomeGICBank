using AwesomeGICBank.Application.Dtos;

namespace AwesomeGICBank.Application.Contracts
{
    public interface IInterestRuleService
    {
        Task<InterestRuleDto> CreateInterestRuleAsync(CreateInterestRuleRequest createInterestRuleRequest);

        Task<List<InterestRuleDto>> GetAllRulesAsync();
    }
}
