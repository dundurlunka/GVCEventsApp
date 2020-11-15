using System;
using System.ComponentModel.DataAnnotations;

namespace WebLayer.Validators
{
    public class DateInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date;
            bool isValid = DateTime.TryParse(value.ToString(), out date);

            if (!isValid)
            {
                return new ValidationResult("Invalid date format");
            }
            else if (date < DateTime.Now)
            {
                return new ValidationResult("StartDate must be in the future.");
            }

            return ValidationResult.Success;
        }
    }
}
