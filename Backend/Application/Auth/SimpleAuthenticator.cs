using FamilyBudgetDomain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetDomain.Interfaces;

namespace FamilyBudgetApplication.Auth
{
    public class SimpleAuthenticator: IAuthenticator
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public SimpleAuthenticator(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new AuthenticationException("There's no user with given username");

            if (!await _userManager.CheckPasswordAsync(user, password))
                throw new AuthenticationException("Wrong password");

            return user;
        }
    }
}