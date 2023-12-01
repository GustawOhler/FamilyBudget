using System.Linq.Expressions;
using FamilyBudgetDomain.Interfaces.Database;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : IEntityBase
    {
        public BaseSpecification()
        {
        }
        public List<Expression<Func<T, bool>>> Criteria { get; } = new List<Expression<Func<T, bool>>>();
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddCriteria(Expression<Func<T, bool>> criterion){
            Criteria.Add(criterion);
        }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
