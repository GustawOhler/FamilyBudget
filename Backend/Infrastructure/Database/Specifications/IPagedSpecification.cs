using System.Linq.Expressions;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public interface IPagedSpecification<T> : ISpecification<T> where T : IEntityBase
    {
        int PageNumber { get; }
        int PageSize { get; }
    }
}