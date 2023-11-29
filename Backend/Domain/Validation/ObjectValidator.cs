using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FamilyBudgetDomain.Validation
{
    public class ObjectValidator : IObjectValidator
    {
        public void Validate(object entity)
        {
            if (entity is IValidatableObject)
            {
                var validatableObject = (IValidatableObject)entity;
                var validationErrors = validatableObject.Validate(new ValidationContext(entity)).ToList();
                if (validationErrors.Count > 0)
                {
                    var validationMessages = new StringBuilder();
                    for (int i = 0; i < validationErrors.Count; i++)
                    {
                        validationMessages.AppendLine(validationErrors[i].ToString());
                    }
                    throw new Exceptions.ValidationException(validationMessages.ToString());
                }
            }
        }
    }
}