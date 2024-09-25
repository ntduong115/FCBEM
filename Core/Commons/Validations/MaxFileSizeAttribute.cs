using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace Core.Commons.Validations
{
    public class MaxFileSizeAttribute(int maxFileSize) : ValidationAttribute
    {
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is {maxFileSize} bytes.";
        }
    }
}
