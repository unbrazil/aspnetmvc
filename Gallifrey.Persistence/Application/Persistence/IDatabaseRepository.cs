﻿using System.Data.Entity;

namespace Gallifrey.Persistence.Application.Persistence
{
    public interface IDatabaseRepository<TModel, in TIdentityType> : IRepository<TModel, TIdentityType>,
        IRepositoryFilters<TModel>
        where TModel : class
    {
        void DisableProxyAndLazyLoading();

        DbContext GetContext();

        DbSet<TModel> GetDbSet();
    }
}