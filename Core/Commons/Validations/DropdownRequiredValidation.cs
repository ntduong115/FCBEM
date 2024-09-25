using System.ComponentModel.DataAnnotations;

namespace Core.Commons.Validations
{
    public class DropdownRequiredValidation : ValidationAttribute
    {
        public DropdownRequiredValidation()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value.ToString().Equals("") || value.ToString().Equals("-1"))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            return null;

        }
    }
}
