using System;
using System.Linq;

namespace Gallifrey.Persistence.Application.Persistence
{
    public interface IRepository
    {

    }

    public interface IRepository<TModel, in TId> : IRepository where TModel : class
    {
        event EventHandler<TModel> BeforeInsertEvent;

        event EventHandler<ModelChangeTracker<TModel>> BeforeUpdateEvent;

        event EventHandler AfterSaveEvent;

        TModel Find(TId id);

        IQueryable<TModel> GetAll();

        void InsertOrUpdate(TModel model);

        void Delete(TId id);

        void Save();
    }
}