using System;

namespace FamilyBudgetUI.Backend.DTOs
{
    public class AuthorizationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public int Id { get; set; }
    }
}
