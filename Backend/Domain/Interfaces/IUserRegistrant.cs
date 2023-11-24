using System;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetDomain.Interfaces
{
    public interface IUserRegistrant
    {
        public Task<User> RegisterUser(string username, string password);
    }
}
