using System.Linq.Expressions;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetDomain.Interfaces.Database
{
    public interface IPagedSpecification<T> : ISpecification<T> where T : IEntityBase
    {
        int PageNumber { get; }
        int PageSize { get; }
    }
}