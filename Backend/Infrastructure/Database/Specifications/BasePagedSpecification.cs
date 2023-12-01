using FamilyBudgetDomain.Interfaces.Database;
using FamilyBudgetDomain.Models;

namespace FamilyBudgetInfrastructure.Database.Specifications
{
    public class BasePagedSpecification<T> : BaseSpecification<T>, IPagedSpecification<T> where T : IEntityBase
    {
        public int PageNumber { get; init; }

        public int PageSize { get; init; }

        public BasePagedSpecification(int pageNumber, int pageSize) : base()
        {
            PageNumber=pageNumber;
            PageSize=pageSize;
        }
    }
}
