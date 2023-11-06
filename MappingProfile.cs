using AutoMapper;
using FamilyBudget.DTOs;
using FamilyBudget.Models;

namespace FamilyBudget
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserOutputModel>();
            CreateMap<BalanceChange, BalanceChangeResponse>();
            CreateMap<Budget, BudgetResponse>();
            CreateMap<BalanceChangeRequest, BalanceChange>();
        }
    }
}