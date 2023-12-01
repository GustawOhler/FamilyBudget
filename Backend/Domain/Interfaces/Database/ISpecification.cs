using System.Linq.Expressions;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetDomain.Interfaces.Database
{
    public interface ISpecification<T> where T : IEntityBase
    {
        List<Expression<Func<T, bool>>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}