using System.Linq.Expressions;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public interface ISpecification<T> where T : IEntityBase
    {
        List<Expression<Func<T, bool>>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }
}