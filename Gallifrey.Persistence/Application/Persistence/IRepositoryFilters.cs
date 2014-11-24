using System.Linq;

namespace Gallifrey.Persistence.Application.Persistence
{
    public interface IRepositoryFilters<TModel>
    {
        bool IsFindFiltered { get; set; }

        IQueryable<TModel> GetAllFiltered();

        IQueryable<TModel> ApplyFilterAndOrdering(IQueryable<TModel> enumerable);
    }
}