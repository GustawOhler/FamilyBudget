using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public class BudgetSpecification : BaseSpecification<Budget>
    {
        public BudgetSpecification(int budgetId)
        {
            AddCriteria(b => b.Id == budgetId);
            AddInclude(b => b.BalanceChanges);
            AddInclude(b => b.Members);
            AddInclude(b => b.Admin);
        }
    }
}
