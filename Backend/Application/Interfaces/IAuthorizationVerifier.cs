using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Interfaces

{
    public interface IAuthorizationVerifier
    {
        public bool CheckAuthorizationForUser(User user, int requestedUserId);
        public bool CheckAuthorizationForUser(User user, string requestingUsername);
        public bool CheckAuthorizationForBudget(User user, Budget budget);
        public bool IsAdmin(User user, Budget budget);
    }
}
