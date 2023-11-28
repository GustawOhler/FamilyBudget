using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public class PagedBalanceChangesSpecification : BasePagedSpecification<BalanceChange>
    {
        public PagedBalanceChangesSpecification(Budget budget, int pageNumber, int pageSize) : base(pageNumber, pageSize)
        {
            AddCriteria(bc => bc.Budget == budget);
        }
    }
}