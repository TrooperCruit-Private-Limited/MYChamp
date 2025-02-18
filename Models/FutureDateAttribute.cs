using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                var model = (DHolidays)validationContext.ObjectInstance;
                // If it's an update (i.e., the Id is set), skip the date validation
                if (model.Id > 0) // Assuming ID > 0 means it's an update
                {
                    return ValidationResult.Success;
                }

                // For creation, enforce future date validation
                if (dateValue <= DateTime.Now.Date)
                {
                    return new ValidationResult(ErrorMessage ?? "Date must be in the future.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
