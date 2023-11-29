using Application.BudgetOperations.DTOs;
using AutoMapper;
using FamilyBudgetApplication.BalanceChangeOperations.DTOs;
using FamilyBudgetApplication.BudgetOperations.DTOs;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetApplication
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewBalanceChangeInput, BalanceChange>();
            CreateMap<NewBudgetInput, Budget>();
        }
    }
}