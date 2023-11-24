using FamilyBudgetDomain.Interfaces;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Auth
{
    public class AuthorizationVerifier : IAuthorizationVerifier
    {
        public bool CheckAuthorizationForUser(User user, int requestedUserId)
        {
            if (user == null || user.Id != requestedUserId)
            {
                return false;
            }
            return true;
        }

        public bool CheckAuthorizationForBudget(User user, Budget budget)
        {
            if (user == null || !budget.Members.Any(u => u.Id == user.Id))
            {
                return false;
            }
            return true;
        }
    }
}