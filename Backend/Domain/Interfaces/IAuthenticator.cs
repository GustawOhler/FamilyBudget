using FamilyBudgetDomain.Models;

namespace FamilyBudgetDomain.Interfaces
{
    public interface IAuthenticator
    {
        public Task<User> AuthenticateUser(string username, string password);
    }
}
