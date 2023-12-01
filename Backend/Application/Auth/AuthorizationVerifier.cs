using FamilyBudgetApplication.Interfaces;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Auth
{
    public class AuthorizationVerifier : IAuthorizationVerifier
    {
        public bool IsAuthorizedForUser(User user, int requestedUserId)
        {
            if (user == null || user.Id != requestedUserId)
            {
                return false;
            }
            return true;
        }

        public bool IsAuthorizedForUser(User user, string requestingUsername)
        {
            if (user == null || !user.UserName.Equals(requestingUsername))
            {
                return false;
            }
            return true;
        }

        public bool IsBudgetMember(User user, Budget budget)
        {
            if (user == null || !budget.Members.Any(u => u.Id == user.Id))
            {
                return false;
            }
            return true;
        }

        public bool IsAdmin(User user, Budget budget)
        {
            if (user == null || budget.Admin != user)
            {
                return false;
            }
            return true;
        }
    }
}