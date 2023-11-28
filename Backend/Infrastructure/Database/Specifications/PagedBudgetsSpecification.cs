using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public class PagedBudgetsSpecification : BasePagedSpecification<Budget>
    {
        public PagedBudgetsSpecification(User member, string name, int pageNumber, int pageSize) : base(pageNumber, pageSize)
        {
            AddCriteria(b => b.Members.Contains(member));
            if (name?.Length > 0)
            {
                AddCriteria(b => b.Name.Contains(name));
            }
            AddInclude(b => b.Members);
        }
    }
}