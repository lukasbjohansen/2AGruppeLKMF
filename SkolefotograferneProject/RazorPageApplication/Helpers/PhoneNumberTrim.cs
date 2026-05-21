using System.Text.RegularExpressions;

namespace RazorPageApplication.Helpers
{
    public static class PhoneNumberTrim
    {
        public static string TrimPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;

            // Remove all non-digit characters
            string digitsOnly = Regex.Replace(phoneNumber, @"\D", "");

            // Return only the last 8 digits
            return digitsOnly.Length >= 8
                ? digitsOnly[^8..]
                : digitsOnly;
        }
    }
}
