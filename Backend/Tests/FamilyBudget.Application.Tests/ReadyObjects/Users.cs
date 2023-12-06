using FamilyBudgetDomain.Models;

namespace FamilyBudget.Application.Tests.ReadyObjects
{
    public class Users
    {
        public const string VALID_USERNAME = "validUser";
        public const string VALID_PASSWORD = "validPassword";
        public const int VALID_USER_ID = 1;
        private static User _validUser = new User
        {
            Id = VALID_USER_ID,
            UserName = VALID_USERNAME
        };

        public static User ValidUser
        {
            get
            {
                return _validUser;
            }
        }
    }
}