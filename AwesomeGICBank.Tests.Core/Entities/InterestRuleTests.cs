using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Tests.Core
{
    public class InterestRuleTests
    {
        [Fact]
        public void Create_InterestRule_ShouldSetProperties()
        {
            // Arrange
            var ruleDate = new DateTime(2023, 10, 25);

            // Act
            var interestRule = new InterestRule()
            {
                Date = ruleDate,
                Rate = 1.2m,
                RuleId = "RULE001"
            };

            // Assert
            Assert.Equal(ruleDate, interestRule.Date);
            Assert.Equal(1.2m, interestRule.Rate);
            Assert.Equal("RULE001", interestRule.RuleId);
        }
    }
}