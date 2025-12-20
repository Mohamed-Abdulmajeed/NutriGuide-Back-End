using System.ComponentModel.DataAnnotations;

namespace NutriGuide.Custom_Validation
{
    public class FutureDateOnlyAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateOnly date)
            {
                return date >= DateOnly.FromDateTime(DateTime.Today);
            }
            return false;
        }

    }
}
