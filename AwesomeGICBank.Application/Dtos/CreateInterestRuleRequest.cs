namespace AwesomeGICBank.Application.Dtos
{
    public class CreateInterestRuleRequest : RequestBase
    {
        public string RuleId { get; set; } = string.Empty;

        public decimal RatePercentage { get; set; }
    }
}
