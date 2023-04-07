using System.ComponentModel.DataAnnotations;

namespace Proizvodii.Attribute
{
    public class RequiredIf : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }

        public RequiredIf(string propertyName, object desiredvalue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredvalue;
        }


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;

            var type = instance.GetType();

            var proprtyvalue = type.GetProperty(PropertyName)?.GetValue(instance, null);


            if (proprtyvalue?.ToString() == DesiredValue.ToString()
                && string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
