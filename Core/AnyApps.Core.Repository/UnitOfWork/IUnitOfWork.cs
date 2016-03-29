using System;
using System.Data;
using AnyApps.Core.Repository.Infrastructure;
using AnyApps.Core.Repository.Repositories;

namespace AnyApps.Core.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
    }
}