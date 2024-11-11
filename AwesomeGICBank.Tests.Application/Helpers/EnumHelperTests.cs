using AwesomeGICBank.Application.Helpers;
using AwesomeGICBank.Core.Enums;

namespace AwesomeGICBank.Tests.Application.Helpers
{
    public class EnumHelperTests
    {
        [Theory]
        [InlineData("D", TransactionType.D)]
        [InlineData("W", TransactionType.W)]
        [InlineData("d", TransactionType.D)]
        [InlineData("w", TransactionType.W)]
        public void TryParseEnum_ValidValue_ReturnsTrueAndCorrectEnum(string value, TransactionType expected)
        {
            // Act
            bool result = EnumHelper.TryParseEnum(value, out TransactionType parsedEnum);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, parsedEnum);
        }

        [Theory]
        [InlineData("Deposit")]
        [InlineData("Withdraw")]
        [InlineData(null)]
        public void TryParseEnum_InvalidValue_ReturnsFalseAndDefaultEnum(string value)
        {
            // Act
            bool result = EnumHelper.TryParseEnum(value, out TransactionType parsedEnum);

            // Assert
            Assert.False(result);
            Assert.Equal(default, parsedEnum);
        }
    }
}