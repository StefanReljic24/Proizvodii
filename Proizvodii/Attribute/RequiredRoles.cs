using Proizvodii.Models;
using System.ComponentModel.DataAnnotations;

namespace Proizvodii.Attribute
{
    public class RequiredRoles : RequiredAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            
            var collection = value as List<RoleModel>;

            if (collection == null)
                return new ValidationResult(ErrorMessage);

            
            if (!collection.Any(p => p.Selected))
            {
               
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
