using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Interfaces;
using FamilyBudgetDomain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth
{
    public class UserRegistrant : IUserRegistrant
    {
        private readonly UserManager<User> _userManager;

        public UserRegistrant(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> RegisterUser(string username, string password)
        {
            var userExists = await _userManager.FindByNameAsync(username);
            if (userExists != null)
                throw new RegistrationException("User already exists");

            var user = new User()
            {
                UserName = username
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InnerQuietException(result.Errors);
            return user;
        }
    }
}
