using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using FamilyBudget.Common;
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
            if (!AuthorizationChecker.CheckAuthorizationForUser(authUser, userId))
            {
                return Unauthorized("Authorized user is not identical to one who's resources are requested.");
            }

            var requestedUser = await dbContext.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();

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

            var budgets = await budgetsQuery.ToListAsync();

            return Content(CustomJsonSerializer.SerializeIgnoreCycles(new { budgets = mapper.Map<List<BudgetResponse>>(budgets) }), "application/json");
        }

        [HttpGet]
        [Route("{budgetId}")]
        public async Task<IActionResult> GetBudgetDetails([FromRoute] int budgetId)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var requestedBudget = await dbContext.Budgets.Include(b => b.BalanceChanges).Include(b => b.Members).Include(b => b.Admin).Where(b => b.Id == budgetId).SingleAsync();

            if (!AuthorizationChecker.CheckAuthorizationForBudget(authUser, requestedBudget))
            {
                return Unauthorized("User is not one of budget members.");
            }

            return Content(CustomJsonSerializer.SerializeIgnoreCycles(mapper.Map<BudgetResponse>(requestedBudget)), "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget([FromBody] BudgetCreationRequest budgetCreationData)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);

            var budget = mapper.Map<Budget>(budgetCreationData);
            budget.Admin = authUser;
            budget.Members.Add(authUser);

            dbContext.Budgets.Add(budget);
            await dbContext.SaveChangesAsync();

            return Content(CustomJsonSerializer.SerializeIgnoreCycles(mapper.Map<BudgetResponse>(budget)), "application/json");
        }

        [HttpPost]
        [Route("{budgetId}/balanceChanges/search")]
        public async Task<IActionResult> SearchBalanceChanges([FromRoute] int budgetId, [FromBody] BalanceChangeSearchRequest searchRequest)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var requestedBudget = await dbContext.Budgets.Include(b => b.BalanceChanges).Include(b => b.Members).Include(b => b.Admin).Where(b => b.Id == budgetId).SingleAsync();

            if (!AuthorizationChecker.CheckAuthorizationForBudget(authUser, requestedBudget))
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

            var balanceChanges = await query.ToListAsync();

            return Content(JsonSerializer.Serialize(new { budgets = mapper.Map<List<BalanceChangeResponse>>(balanceChanges) }), "application/json");
        }

        [HttpPost]
        [Route("{budgetId}/balanceChanges")]
        public async Task<IActionResult> AddBalanceChange([FromRoute] int budgetId, [FromBody] BalanceChangeRequest balanceChangeRequest)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var budget = await dbContext.Budgets.Include(b => b.Members).Where(b => b.Id == budgetId).SingleAsync();

            if (!AuthorizationChecker.CheckAuthorizationForBudget(authUser, budget))
            {
                return Unauthorized("User is not one of budget members.");
            }

            var balanceChangeDb = mapper.Map<BalanceChange>(balanceChangeRequest);
            balanceChangeDb.Budget = budget;
            balanceChangeDb.Category = await dbContext.Categories.Where(c => c.Id == balanceChangeRequest.CategoryId).SingleOrDefaultAsync();

            if (balanceChangeDb.Type == Common.BalanceChangeType.Income)
            {
                budget.Balance += balanceChangeDb.Amount;
            }
            else
            {
                budget.Balance -= balanceChangeDb.Amount;
            }

            dbContext.BalanceChanges.Add(balanceChangeDb);
            await dbContext.SaveChangesAsync();

            return Content(CustomJsonSerializer.SerializeIgnoreCycles(mapper.Map<BalanceChangeResponse>(balanceChangeDb)), "application/json");
        }

        [HttpPatch]
        [Route("{budgetId}/users")]
        public async Task<IActionResult> AddUsersToBudget([FromRoute] int budgetId, [FromBody] UserListRequest newMembers)
        {
            var authUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var budget = await dbContext.Budgets.Include(b => b.Members).Where(b => b.Id == budgetId).SingleAsync();

            if (!AuthorizationChecker.CheckAuthorizationForBudget(authUser, budget))
            {
                return Unauthorized("User is not one of budget members.");
            }

            if (budget.AdminId != authUser.Id)
            {
                return Unauthorized("User does not have rights to perform this operation");
            }

            var newMembersDb = await dbContext.Users.Where(u => newMembers.UserIds.Contains(u.Id)).ToListAsync();

            var currentMembers = budget.Members.ToList();

            var concatMembers = currentMembers;
            concatMembers.AddRange(newMembersDb);
            budget.Members = concatMembers.Distinct().ToList();
            dbContext.SaveChanges();

            return Content(CustomJsonSerializer.SerializeIgnoreCycles(mapper.Map<BudgetResponse>(budget)), "application/json");
        }
    }
}