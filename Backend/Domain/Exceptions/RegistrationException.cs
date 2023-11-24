using System.Runtime.Serialization;
using System.Text;

namespace FamilyBudgetDomain.Exceptions
{
    public class RegistrationException : Exception
    {
        public RegistrationException()
        {
        }

        public RegistrationException(string? message) : base(message)
        {
        }

        public RegistrationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}