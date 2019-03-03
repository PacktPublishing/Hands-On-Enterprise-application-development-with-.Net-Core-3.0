using System.ComponentModel.DataAnnotations;

namespace Draken.Data.Validators
{
    public class BusinessOnlyEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var email = value.ToString().ToLower();
            return !email.Contains("hotmail") && !email.Contains("gmail");
        }
    }
}
