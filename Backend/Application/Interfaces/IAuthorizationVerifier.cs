using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Interfaces

{
    public interface IAuthorizationVerifier
    {
        public bool IsAuthorizedForUser(User user, int requestedUserId);
        public bool IsAuthorizedForUser(User user, string requestingUsername);
        public bool IsBudgetMember(User user, Budget budget);
        public bool IsAdmin(User user, Budget budget);
    }
}
