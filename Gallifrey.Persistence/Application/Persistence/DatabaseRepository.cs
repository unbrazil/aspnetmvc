using System;
using System.Data.Entity;
using System.Linq;
using Gallifrey.Persistence.Application.Extension;
using Omu.ValueInjecter;

namespace Gallifrey.Persistence.Application.Persistence
{
    public abstract class DatabaseRepository<TModel, TIdentityType> : IDatabaseRepository<TModel, TIdentityType>
        where TModel : class, IIdentity<TIdentityType>
        where TIdentityType : struct
    {
        public bool IsFindFiltered { get; set; }

        public event EventHandler<TModel> BeforeInsertEvent;

        public event EventHandler<ModelChangeTracker<TModel>> BeforeUpdateEvent;

        public event EventHandler AfterSaveEvent;

        private readonly DbContext _context;

        protected DatabaseRepository(DbContext context)
        {
            _context = context;
            IsFindFiltered = true;
        }

        public virtual DbContext GetContext()
        {
            return _context;
        }

        public virtual DbSet<TModel> GetDbSet()
        {
            return GetContext().GetDbSet<TModel>();
        }

        public virtual IQueryable<TModel> GetAll()
        {
            return GetDbSet();
        }

        public virtual IQueryable<TModel> GetAllFiltered()
        {
            return ApplyFilterAndOrdering(GetAll());
        }

        /// <summary>
        /// Override to apply custom ordering/filtering
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public virtual IQueryable<TModel> ApplyFilterAndOrdering(IQueryable<TModel> enumerable)
        {
            return enumerable;
        }

        public TModel Find(TIdentityType id)
        {
            return (IsFindFiltered ? GetAllFiltered() : GetAll()).SingleOrDefault(r => r.Id.Equals(id));
        }

        public void InsertOrUpdate(TModel model)
        {
            if (model.Id.Equals(default(TIdentityType)))
            {
                if (BeforeInsertEvent != null)
                    BeforeInsertEvent(this, model);

                GetDbSet().Add(model);
            }
            else
            {
                var modelFromDatabase = Find(model.Id);

                if (BeforeUpdateEvent != null)
                    BeforeUpdateEvent(this, new ModelChangeTracker<TModel>
                    {
                        New = model,
                        Old = modelFromDatabase
                    });

                modelFromDatabase.InjectFrom(model);
            }
        }

        public void Delete(TIdentityType id)
        {
            GetDbSet().Remove(Find(id));
        }

        public void DisableProxyAndLazyLoading()
        {
            GetContext().Configuration.ProxyCreationEnabled = false;
            GetContext().Configuration.LazyLoadingEnabled = false;
        }

        public void Save()
        {
            GetContext().SaveChanges();

            if (AfterSaveEvent != null)
                AfterSaveEvent(this, new EventArgs());
        }
    }
}