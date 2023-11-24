using System;
using System.Linq.Expressions;
using FamilyBudget;
using FamilyBudgetDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class StandardRepository<T> : IRepository<T> where T : class, IEntityBase
    {
        public readonly FamilyBudgetDbContext _dbContext;

        public StandardRepository(FamilyBudgetDbContext context){
            _dbContext = context;
        }

        public void Add(T entity)
        {
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Edit(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public T? GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IEnumerable<T> List()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
               .Where(predicate)
               .AsEnumerable();
        }
    }
}
