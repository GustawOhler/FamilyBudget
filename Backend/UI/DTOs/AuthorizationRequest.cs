namespace FamilyBudgetUI.DTOs
{
    public class AuthorizationRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}