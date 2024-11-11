using System.Globalization;

namespace AwesomeGICBank.Application.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsInRange(decimal value, decimal min, decimal max)
        {
            return value > min && value < max;
        }

        public static bool TryParseDate(string dateString, string format, out DateTime parsedDate) =>
            DateTime.TryParseExact(dateString, format, null, DateTimeStyles.None, out parsedDate);

        public static bool TryParseDecimal(string decimalString, out decimal parsedDecimal) =>
            decimal.TryParse(decimalString, NumberStyles.Number, CultureInfo.InvariantCulture, out parsedDecimal);
    }
}
