using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Interfaces
{
    public interface IAuthenticator
    {
        public Task<User> AuthenticateUser(string username, string password);
    }
}
