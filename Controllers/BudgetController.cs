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

        [HttpPost]
        [Route("~/api/users/{userId}/budgets/search")]
        public async Task<IActionResult> GetBudgetsForUser([FromRoute] int userId, [FromBody] BudgetSearchRequest budgetSearchRequest)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            if (authUser == null || authUser.Id != userId)
            {
                return Unauthorized("Authorized user is not identical to one who's resources are requested.");
            }

            var requestedUser = dbContext.Users.Where(u => u.Id == userId).SingleOrDefault();

            if (requestedUser == null)
            {
                return NotFound("Given user does not exist");
            }

            if (budgetSearchRequest.Limit > 250)
            {
                return BadRequest("Can't retrieve more than 250 rows");
            }

            var budgetsQuery = dbContext.Budgets
                .Include(b => b.BalanceChanges).Include(b => b.Members)
                .Where(b => b.Members.Contains(requestedUser))
                .Skip(budgetSearchRequest.Offset)
                .Take(budgetSearchRequest.Limit);

            if (budgetSearchRequest.Name != null)
            {
                budgetsQuery = budgetsQuery.Where(b => b.Name.Contains(budgetSearchRequest.Name));
            }

            var budgets = budgetsQuery.ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Content(JsonSerializer.Serialize(new { budgets = mapper.Map<List<BudgetResponse>>(budgets) }, options), "application/json");
        }

        [HttpGet]
        [Route("{budgetId}")]
        public async Task<IActionResult> GetBudgetDetails([FromRoute] int budgetId)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var requestedBudget = dbContext.Budgets.Include(b => b.BalanceChanges).Include(b => b.Members).Include(b => b.Admin).Where(b => b.Id == budgetId).Single();

            if (authUser == null || !requestedBudget.Members.Any(u => u.Id == authUser.Id))
            {
                return Unauthorized("User is not one of budget members.");
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Content(JsonSerializer.Serialize(mapper.Map<BudgetResponse>(requestedBudget), options), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget([FromBody] BudgetCreationRequest budgetCreationData)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);

            var budget = mapper.Map<Budget>(budgetCreationData);
            budget.Admin = authUser;
            budget.Members.Add(authUser);

            dbContext.Budgets.Add(budget);
            dbContext.SaveChanges();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Content(JsonSerializer.Serialize(mapper.Map<BudgetResponse>(budget), options), "application/json");
        }

        [HttpPost]
        [Route("{budgetId}/balanceChanges/search")]
        public async Task<IActionResult> SearchBalanceChanges([FromRoute] int budgetId, [FromBody] BalanceChangeSearchRequest searchRequest)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var requestedBudget = dbContext.Budgets.Include(b => b.BalanceChanges).Include(b => b.Members).Include(b => b.Admin).Where(b => b.Id == budgetId).Single();

            if (authUser == null || !requestedBudget.Members.Any(u => u.Id == authUser.Id))
            {
                return Unauthorized("User is not one of budget members.");
            }

            if (searchRequest.Limit > 250)
            {
                return BadRequest("Can't retrieve more than 250 rows");
            }

            var query = dbContext.BalanceChanges.Where(bc => bc.Budget == requestedBudget).Skip(searchRequest.Offset).Take(searchRequest.Limit);

            if (searchRequest.Name != null)
            {
                query = query.Where(bc => bc.Name.Contains(searchRequest.Name));
            }

            var balanceChanges = query.ToList();

            return Content(JsonSerializer.Serialize(new { budgets = mapper.Map<List<BalanceChangeResponse>>(balanceChanges) }), "application/json");
        }

        [HttpPost]
        [Route("{budgetId}/balanceChanges")]
        public async Task<IActionResult> AddBalanceChange([FromRoute] int budgetId, [FromBody] BalanceChangeRequest balanceChangeRequest)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var budget = dbContext.Budgets.Include(b => b.Members).Where(b => b.Id == budgetId).Single();

            if (authUser == null || !budget.Members.Any(u => u.Id == authUser.Id))
            {
                return Unauthorized("User is not one of budget members.");
            }

            var balanceChangeDb = mapper.Map<BalanceChange>(balanceChangeRequest);
            balanceChangeDb.Budget = budget;
            balanceChangeDb.Category = dbContext.Categories.Where(c => c.Id == balanceChangeRequest.CategoryId).SingleOrDefault();

            budget.Balance += balanceChangeDb.Type == Common.BalanceChangeType.Income ? balanceChangeDb.Amount : -balanceChangeDb.Amount;

            dbContext.BalanceChanges.Add(balanceChangeDb);
            dbContext.SaveChanges();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Content(JsonSerializer.Serialize(mapper.Map<BalanceChangeResponse>(balanceChangeDb), options), "application/json");
        }

        [HttpPatch]
        [Route("{budgetId}/users")]
        public async Task<IActionResult> AddUsersToBudget([FromRoute] int budgetId, [FromBody] UserListRequest newMembers)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var budget = dbContext.Budgets.Include(b => b.Members).Where(b => b.Id == budgetId).Single();

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

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Content(JsonSerializer.Serialize(mapper.Map<BudgetResponse>(budget), options), "application/json");
        }
    }
}