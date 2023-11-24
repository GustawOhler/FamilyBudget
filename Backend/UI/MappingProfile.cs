using AutoMapper;
using FamilyBudget.DTOs;
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
            CreateMap<Budget, BudgetResponse>();
            CreateMap<BalanceChangeRequest, BalanceChange>();
            CreateMap<Category, CategoryResponse>();
        }
    }
}