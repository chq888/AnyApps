using System;
using System.Threading;
using System.Threading.Tasks;
using AnyApps.Core.Repository.Infrastructure;
using AnyApps.Core.Repository.Repositories;

namespace AnyApps.Core.Repository.UnitOfWork
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState;
    }
}