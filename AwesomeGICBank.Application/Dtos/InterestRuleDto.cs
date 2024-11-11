namespace AwesomeGICBank.Application.Dtos
{
    public class InterestRuleDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string RuleId { get; set; } = string.Empty;

        public decimal InterestRate { get; set; }
    }
}
