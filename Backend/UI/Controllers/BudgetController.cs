using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using FamilyBudgetUI.Common;
using FamilyBudgetUI.DTOs;
using FamilyBudgetApplication.BudgetOperations;
using FamilyBudgetDomain.Exceptions;
using FamilyBudgetApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Application.BudgetOperations.DTOs;
using FamilyBudgetApplication.BudgetOperations.DTOs;
using FamilyBudgetApplication.BalanceChangeOperations.DTOs;

namespace FamilyBudgetUI.Controllers
{
    [Route("api/budgets")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBudgetRetriever _budgetRetriever;
        private readonly IBalanceChangeManager _entityCreator;
        private readonly IBalanceChangesRetriever _balanceChangesRetriever;
        private readonly IBudgetManager _budgetManager;

        public BudgetController(IMapper mapper, IBudgetRetriever budgetRetriever, IBalanceChangeManager entityCreator, IBalanceChangesRetriever balanceChangesRetriever, IBudgetManager budgetManager)
        {
            _mapper = mapper;
            _budgetRetriever = budgetRetriever;
            _entityCreator = entityCreator;
            _balanceChangesRetriever = balanceChangesRetriever;
            _budgetManager = budgetManager;
        }

        [HttpGet]
        [Route("~/api/users/{userId}/budgets")]
        public async Task<IActionResult> GetBudgetsForUser([FromRoute] int userId, [FromQuery] BudgetSearchQuery budgetSearchQuery)
        {
            try
            {
                var budgetSearchInput = _mapper.Map<BudgetsForUserInput>(budgetSearchQuery);
                budgetSearchInput.RequestedUserId = userId;
                budgetSearchInput.RequestingUserName = User.Identity.Name;

                var budgets = await _budgetRetriever.GetBudgetsForUser(budgetSearchInput);

                return Content(CustomJsonSerializer.SerializeIgnoreCycles(new { budgets = _mapper.Map<List<BudgetResponse>>(budgets) }), "application/json");
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AuthorizationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet]
        [Route("{budgetId}")]
        public async Task<IActionResult> GetBudgetDetails([FromRoute] int budgetId)
        {
            try
            {
                var budget = await _budgetRetriever.GetBudget(User.Identity.Name, budgetId);
                return Content(CustomJsonSerializer.SerializeIgnoreCycles(_mapper.Map<BudgetResponse>(budget)), "application/json");
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AuthorizationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget([FromBody] BudgetCreationRequest budgetCreationData)
        {
            var budgetInput = _mapper.Map<NewBudgetInput>(budgetCreationData);
            budgetInput.AdminUsername = User.Identity.Name;
            var budget = await _budgetManager.CreateBudget(budgetInput);

            return Content(CustomJsonSerializer.SerializeIgnoreCycles(_mapper.Map<BudgetResponse>(budget)), "application/json");
        }

        [HttpGet]
        [Route("{budgetId}/balanceChanges")]
        public async Task<IActionResult> GetBalanceChanges([FromRoute] int budgetId, [FromQuery] BalanceChangeSearchQuery searchQuery)
        {
            try
            {
                var balanceChangeSearchInput = _mapper.Map<BalanceChangesForBudgetInput>(searchQuery);
                balanceChangeSearchInput.RequestingUserName = User.Identity.Name;
                balanceChangeSearchInput.BudgetId = budgetId;

                var balanceChanges = await _balanceChangesRetriever.GetBalanceChangesForBudget(balanceChangeSearchInput);
                return Content(CustomJsonSerializer.SerializeIgnoreCycles(new { budgets = _mapper.Map<List<BalanceChangeResponse>>(balanceChanges) }), "application/json");
            }
            catch (AuthorizationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost]
        [Route("{budgetId}/balanceChanges")]
        public async Task<IActionResult> AddBalanceChange([FromRoute] int budgetId, [FromBody] BalanceChangeRequest balanceChangeRequest)
        {
            try
            {
                var balanceChangeInput = _mapper.Map<NewBalanceChangeInput>(balanceChangeRequest);
                balanceChangeInput.BudgetId = budgetId;
                balanceChangeInput.RequestingUserName = User.Identity.Name;
                var balanceChangeDb = await _entityCreator.CreateBalanceChange(balanceChangeInput);
                return Content(CustomJsonSerializer.SerializeIgnoreCycles(_mapper.Map<BalanceChangeResponse>(balanceChangeDb)), "application/json");
            }
            catch (AuthorizationException authEx)
            {
                return Unauthorized(authEx.Message);
            }
            catch (FamilyBudgetDomain.Exceptions.ValidationException valExc)
            {
                return BadRequest(valExc.Message);
            }
        }

        [HttpPost]
        [Route("{budgetId}/users")]
        public async Task<IActionResult> AddUsersToBudget([FromRoute] int budgetId, [FromBody] UserListRequest newMembers)
        {
            try
            {
                var budgetMembersInput = new NewBudgetMembersInput()
                {
                    BudgetId = budgetId,
                    RequestingUserName = User.Identity.Name,
                    UserIds = newMembers.UserIds
                };
                var budgetWithNewMembers = await _budgetManager.AddBudgetMembers(budgetMembersInput);
                return Content(CustomJsonSerializer.SerializeIgnoreCycles(_mapper.Map<BudgetResponse>(budgetWithNewMembers)), "application/json");
            }
            catch (AuthorizationException authEx)
            {
                return Unauthorized(authEx.Message);
            }
        }
    }
}