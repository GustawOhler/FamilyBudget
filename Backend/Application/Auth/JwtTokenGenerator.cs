using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FamilyBudgetApplication.Interfaces;

namespace FamilyBudgetApplication.Auth
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SecurityToken GenerateToken(string username)
        {
            var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, username)
                };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            return new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                    claims: claims
                    );
        }

        public string TokenToString(SecurityToken token)
        {
            if (token is not JwtSecurityToken)
            {
                throw new ArgumentException("Given token is not JwtSecurityToken type");
            }
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
