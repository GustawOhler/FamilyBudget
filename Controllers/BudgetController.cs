using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using FamilyBudget.DTOs;
using FamilyBudget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FamilyBudget.Controllers
{
    [Route("api/budgets")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly FamilyBudgetDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public BudgetController(UserManager<User> userManager, FamilyBudgetDbContext dbContext, IConfiguration configuration, IMapper mapper)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("~/api/users/{userId}/budgets")]
        public async Task<IActionResult> GetBudgetsForUser([FromRoute] int userId)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            if (authUser == null || authUser.Id != userId)
            {
                return Unauthorized("Authorized user is not identical to one who's resources are requested.");
            }

            var requestedUser = dbContext.Users.Include(u => u.ParticipatingBudgets).ThenInclude(b => b.BalanceChanges).Include(u => u.ParticipatingBudgets).ThenInclude(b => b.Members).Where(u => u.Id == userId).SingleOrDefault();

            if (requestedUser == null)
            {
                return NotFound("Given user does not exist");
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(new { budgets = mapper.Map<List<BudgetResponse>>(requestedUser.ParticipatingBudgets) }, options));
        }

        [HttpPost]
        public async Task<IActionResult> AddBudget([FromBody] BudgetCreationModel budgetCreationData)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);

            var budget = new Budget()
            {
                AdminId = authUser.Id,
                Balance = budgetCreationData.Balance
            };
            budget.Members.Add(authUser);

            dbContext.Budgets.Add(budget);
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("{budgetId}/balanceChange")]
        public async Task<IActionResult> AddBalanceChange([FromRoute] int budgetId, [FromBody] BalanceChangeRequest balanceChangeRequest)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var budget = dbContext.Budgets.Where(b => b.Id == budgetId).Single();

            if (!budget.Members.Contains(authUser!))
            {
                return Unauthorized("User is not a member of this budget.");
            }

            var balanceChangeDb = mapper.Map<BalanceChange>(balanceChangeRequest);
            balanceChangeDb.Budget = budget;

            budget.Balance += balanceChangeDb.Type == Common.BalanceChangeType.Income ? balanceChangeDb.Amount : -balanceChangeDb.Amount;

            dbContext.BudgetChanges.Add(balanceChangeDb);
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpPatch]
        [Route("{budgetId}/users")]
        public async Task<IActionResult> AddUsersToBudget([FromRoute] int budgetId, [FromBody] UserListRequest newMembers)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            Console.WriteLine(authUser.Id);
            var budget = dbContext.Budgets.Include(b => b.Members).Where(b => b.Id == budgetId).Single();
            Console.WriteLine(budget.Members.Count);

            if (!budget.Members.Contains(authUser!))
            {
                return Unauthorized("User is not a member of this budget.");
            }

            var newMembersDb = dbContext.Users.Where(u => newMembers.UserIds.Contains(u.Id)).ToList();

            var currentMembers = budget.Members.ToList();

            var concatMembers = currentMembers;
            concatMembers.AddRange(newMembersDb);
            budget.Members = concatMembers.Distinct().ToList();
            dbContext.SaveChanges();

            return Ok();
        }
    }
}