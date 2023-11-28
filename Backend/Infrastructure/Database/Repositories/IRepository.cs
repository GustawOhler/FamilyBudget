using System.Linq.Expressions;
using FamilyBudgetDomain.Models;
using FamilyBudgetInfrastructure.Database.Specifications;

namespace Infrastructure.Database.Repositories
{
    public interface IRepository<T> where T : IEntityBase
    {
        Task<T?> GetSingleOrDefault(ISpecification<T> specification);
        Task<IEnumerable<T>> List(ISpecification<T> specification);
        Task Add(T entity);
        Task Delete(T entity);
        Task Edit(T entity);
    }
}
