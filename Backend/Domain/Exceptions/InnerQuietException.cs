using System.Runtime.Serialization;
using System.Text;

namespace FamilyBudgetDomain.Exceptions
{
    public class InnerQuietException : Exception
    {
        public InnerQuietException()
        {
        }

        public InnerQuietException(string? message) : base(message)
        {
        }

        public InnerQuietException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        
        public InnerQuietException(IEnumerable<Object> errors) : base(ListOfErrorsToMessage(errors))
        {
        }

        private static string ListOfErrorsToMessage(IEnumerable<Object> errors)
        {
            var exMessageBuilder = new StringBuilder();
            foreach (var error in errors)
            {
                exMessageBuilder.AppendLine(error.ToString());
            }
            return exMessageBuilder.ToString();
        }
    }
}