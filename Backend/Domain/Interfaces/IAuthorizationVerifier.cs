using FamilyBudgetDomain.Models;

namespace FamilyBudgetDomain.Interfaces
{
    public interface IAuthorizationVerifier
    {
        public bool CheckAuthorizationForUser(User user, int requestedUserId);
        public bool CheckAuthorizationForBudget(User user, Budget budget);
    }
}
