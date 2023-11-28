using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(int categoryId)
        {
            AddCriteria(c => c.Id == categoryId);
        }
    }
}