using AwesomeGICBank.Application.Helpers;

namespace AwesomeGICBank.Tests.Application.Helpers
{
    public class ValidationHelperTests
    {
        [Theory]
        [InlineData(5, 1, 10, true)]
        [InlineData(0, 1, 10, false)]
        [InlineData(15, 1, 10, false)]
        public void IsInRange_ShouldReturnExpectedResult(decimal value, decimal min, decimal max, bool expected)
        {
            // Act
            var result = ValidationHelper.IsInRange(value, min, max);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("20240626", "yyyyMMdd", true)]
        [InlineData("2024-06-26", "yyyy-MM-dd", true)]
        [InlineData("26/06/2024", "yyyy-MM-dd", false)]
        [InlineData("2024-11-32", "yyyy-MM-dd", false)]
        [InlineData("abcdef", "yyyy-MM-dd", false)]
        public void TryParseDate_ShouldReturnExpectedResult(string dateString, string format, bool expected)
        {
            // Act
            var result = ValidationHelper.TryParseDate(dateString, format, out DateTime parsedDate);

            // Assert
            Assert.Equal(expected, result);
            if (expected)
            {
                Assert.Equal(new DateTime(2024, 06, 26), parsedDate);
            }
        }

        [Theory]
        [InlineData("123.45", true)]
        [InlineData("abc", false)]
        [InlineData("1000.00", true)]
        public void TryParseDecimal_ShouldReturnExpectedResult(string decimalString, bool expected)
        {
            // Act
            var result = ValidationHelper.TryParseDecimal(decimalString, out decimal parsedDecimal);

            // Assert
            Assert.Equal(expected, result);
            if (expected)
            {
                Assert.Equal(decimal.Parse(decimalString), parsedDecimal);
            }
        }
    }
}
