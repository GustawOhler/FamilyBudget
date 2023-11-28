using System.Runtime.Serialization;

namespace FamilyBudgetDomain.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException()
        {
        }

        public AuthorizationException(string? message) : base(message)
        {
        }
    }
}