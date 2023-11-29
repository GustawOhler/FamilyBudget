using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetDomain.Validation
{
    public interface IObjectValidator
    {
        public void Validate(object entity);
    }
}