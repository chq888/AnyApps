#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using AnyApps.Core.Repository.DataContext;
using AnyApps.Core.Repository.Infrastructure;
using AnyApps.Core.Repository.Repositories;
using AnyApps.Core.Repository.UnitOfWork;
using System.Data.Entity.Infrastructure;

#endregion

namespace AnyApps.Core.Repository.Ef
{
    public class AnyRepository<TEntity> : Repository<TEntity>, IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields

        private readonly IDataContextAsync _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;

        #endregion Private Fields

        public AnyRepository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork) : base(context, unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

            // Temporarily for FakeDbContext, Unit Test and Fakes
            var dbContext = context as DbContext;
            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
            else
            {
                var fakeContext = context as FakeDbContext;

                if (fakeContext != null)
                {
                    _dbSet = fakeContext.Set<TEntity>();
                }
            }
        }

        public override TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public override IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public override void Insert(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public override void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public override void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public override void Update(TEntity entity)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);




        }

        public override void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public override void Delete(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public override IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return new QueryFluent<TEntity>(this, queryObject);
        }

        public override IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        public override async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public override async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public override async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public override async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);

            return true;
        }


        // Insert or Updating an object graph
        [Obsolete("Will be renamed to UpsertGraph(TEntity entity) in next version.")]
        public override void InsertOrUpdateGraph(TEntity entity)
        {
            SyncObjectGraph(entity);
            _entitesChecked = null;
            _dbSet.Attach(entity);
        }

        // tracking of all processed entities in the object graph when calling SyncObjectGraph
        HashSet<object> _entitesChecked; 

        private void SyncObjectGraph(object entity) // scan object graph for all 
        {
            // instantiating _entitesChecked so we can keep track of all entities we have scanned, avoid any cyclical issues
            if(_entitesChecked == null) 
                _entitesChecked = new HashSet<object>(); 

            // if already processed skip
            if (_entitesChecked.Contains(entity))
                return;

            // add entity to alreadyChecked collection
            _entitesChecked.Add(entity);

            var objectState = entity as IObjectState;

            // discovered entity with ObjectState.Added, sync this with provider e.g. EF
            if (objectState != null && objectState.ObjectState == ObjectState.Added)
                _context.SyncObjectState((IObjectState)entity);

            // Set tracking state for child collections
            foreach (var prop in entity.GetType().GetProperties())
            {
                // Apply changes to 1-1 and M-1 properties
                var trackableRef = prop.GetValue(entity, null) as IObjectState;
                if (trackableRef != null)                
                {
                    // discovered entity with ObjectState.Added, sync this with provider e.g. EF
                    if(trackableRef.ObjectState == ObjectState.Added)
                        _context.SyncObjectState((IObjectState) entity);

                    // recursively process the next property
                    SyncObjectGraph(prop.GetValue(entity, null));
                }

                // Apply changes to 1-M properties
                var items = prop.GetValue(entity, null) as IEnumerable<IObjectState>;

                // collection was empty, nothing to process, continue
                if (items == null) continue;

                // collection isn't empty, continue to recursively scan the elements of this collection
                foreach (var item in items)
                    SyncObjectGraph(item);
            }
        }
    }
}