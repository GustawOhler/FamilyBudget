using System;

namespace FamilyBudget.Backend.DTOs
{
    public class AuthorizationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public int Id { get; set; }
    }
}
