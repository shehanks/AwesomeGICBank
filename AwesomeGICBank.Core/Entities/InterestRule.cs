using System.ComponentModel.DataAnnotations;

namespace AwesomeGICBank.Core.Entities
{
    public class InterestRule : EntityBase
    {
        [Required]
        public DateTime Date { get; set; }      // Date the rule applies

        [Required]
        public string RuleId { get; set; } = string.Empty;      // Custom rule identifier, e.g., "RULE01"

        [Required]
        [Range(0.01, 99.99)]
        public decimal Rate { get; set; }
    }
}
