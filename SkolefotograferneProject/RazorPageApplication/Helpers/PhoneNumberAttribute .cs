using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RazorPageApplication.Helpers
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public PhoneNumberAttribute()
        {
            ErrorMessage = "Phonenumber must be a valid format";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            var phone = value.ToString()!;
            var regex = new Regex(@"^(\+\d{2})?\s?[2-9]\d\s?\d{2}\s?\d{2}\s?\d{2}$");

            return regex.IsMatch(phone)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}
