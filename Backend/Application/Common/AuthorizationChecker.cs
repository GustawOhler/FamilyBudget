using FamilyBudgetDomain.Models;

public static class AuthorizationChecker
{
    public static bool CheckAuthorizationForUser(User? user, int requestedUserId)
    {
        if (user == null || user.Id != requestedUserId)
        {
            return false;
        }
        return true;
    }

    public static bool CheckAuthorizationForBudget(User user, Budget budget)
    {
        if (user == null || !budget.Members.Any(u => u.Id == user.Id))
        {
            return false;
        }
        return true;
    }
}