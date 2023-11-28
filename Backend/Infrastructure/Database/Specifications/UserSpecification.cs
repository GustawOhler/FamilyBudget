using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(int userId)
        {
            AddCriteria(u => u.Id == userId);
        }

        public UserSpecification(ICollection<int> userIds)
        {
            AddCriteria(u => userIds.Contains(u.Id));
        }
    }
}
