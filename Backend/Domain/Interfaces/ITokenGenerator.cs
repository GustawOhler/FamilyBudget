using System;
using Microsoft.IdentityModel.Tokens;

namespace FamilyBudgetDomain.Interfaces
{
    public interface ITokenGenerator
    {
        public SecurityToken GenerateToken(string username);
        public string TokenToString(SecurityToken token);
    }
}
