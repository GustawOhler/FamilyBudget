using FamilyBudgetApplication.Common;
using FamilyBudgetDomain.Models;

namespace FamilyBudget.Application.Tests.Common
{
    public class CustomJsonSerializerTests
    {
        [Fact]
        public void SerializeIgnoreCycles_ValidInput_NoException()
        {
            // Arrange
            var budget = new Budget()
            {
                Name = "Test"
            };
            var balanceOperations = new BalanceChange()
            {
                Budget = budget
            };
            budget.BalanceChanges.Add(balanceOperations);

            //Act & Assert
            CustomJsonSerializer.SerializeIgnoreCycles(budget);
        }
    }
}