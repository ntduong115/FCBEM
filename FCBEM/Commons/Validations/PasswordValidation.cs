using System.ComponentModel.DataAnnotations;

namespace FCBEM24.Commons.Validations
{
    public class PasswordValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            return true;

        }
    }
}
