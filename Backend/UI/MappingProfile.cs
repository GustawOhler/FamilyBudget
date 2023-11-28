using Application.BudgetOperations.DTOs;
using AutoMapper;
using FamilyBudget.DTOs;
using FamilyBudgetApplication.BalanceChangeOperations.DTOs;
using FamilyBudgetApplication.BudgetOperations.DTOs;
using FamilyBudgetDomain.Models;

namespace FamilyBudget
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserOutputModel>();
            CreateMap<BalanceChange, BalanceChangeResponse>();
            CreateMap<BudgetCreationRequest, Budget>();
            CreateMap<BalanceChange, BalanceChangeOutputModel>();
            CreateMap<Budget, BudgetResponse>();
            CreateMap<BalanceChangeRequest, BalanceChange>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<BudgetCreationRequest, NewBudgetInput>();
            CreateMap<BalanceChangeRequest, NewBalanceChangeInput>();
            CreateMap<BalanceChangeSearchQuery, BalanceChangesForBudgetInput>();
            CreateMap<BudgetSearchQuery, BudgetsForUserInput>();
        }
    }
}