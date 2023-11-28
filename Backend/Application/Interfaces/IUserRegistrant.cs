using System;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication.Interfaces
{
    public interface IUserRegistrant
    {
        public Task<User> RegisterUser(string username, string password);
    }
}
