namespace AwesomeGICBank.Application.Helpers
{
    public static class EnumHelper
    {
        public static bool TryParseEnum<TEnum>(string value, out TEnum result) where TEnum : struct, Enum
        {
            return Enum.TryParse(value, true, out result); // 'true' makes it case-insensitive
        }
    }
}
