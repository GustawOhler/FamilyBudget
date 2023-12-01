using FamilyBudgetUI;
using FamilyBudgetDomain.Models;
using FamilyBudgetDomain.Validation;
using Microsoft.EntityFrameworkCore;
using FamilyBudgetDomain.Interfaces.Database;

namespace Infrastructure.Database.Repositories
{
    public class StandardRepository<T> : IRepository<T> where T : class, IEntityBase
    {
        public readonly FamilyBudgetDbContext _dbContext;
        public readonly IObjectValidator _validator;

        public StandardRepository(FamilyBudgetDbContext context, IObjectValidator validator)
        {
            _dbContext = context;
            _validator = validator;
        }

        public async Task Add(T entity)
        {
            _validator.Validate(entity);
            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Edit(T entity)
        {
            _validator.Validate(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> List(ISpecification<T> specification)
        {
            var queryableResultWithIncludes = specification.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                            (current, include) => current.Include(include));

            var resultsWithRestIncludes = specification.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                            (current, include) => current.Include(include));

            var finalQuery = specification.Criteria
                .Aggregate(resultsWithRestIncludes,
                            (current, criterion) => current.Where(criterion));

            if (specification is IPagedSpecification<T>)
            {
                var pagingSpec = (IPagedSpecification<T>)specification;
                return await finalQuery
                        .Skip(pagingSpec.PageNumber * pagingSpec.PageSize)
                        .Take(pagingSpec.PageSize)
                        .ToListAsync();
            }
            return await finalQuery.ToListAsync();
        }

        public async Task<T?> GetSingleOrDefault(ISpecification<T> specification)
        {
            var resultsList = await List(specification);
            return resultsList.SingleOrDefault();
        }
    }
}
