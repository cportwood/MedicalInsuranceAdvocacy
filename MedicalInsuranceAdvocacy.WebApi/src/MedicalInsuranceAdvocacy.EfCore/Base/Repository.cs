using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MedicalInsuranceAdvocacy.RepoCore.DataContext;
using MedicalInsuranceAdvocacy.RepoCore.Infrastructure;
using MedicalInsuranceAdvocacy.RepoCore.Interfaces;
using MedicalInsuranceAdvocacy.RepoCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Reflection;

namespace MedicalInsuranceAdvocacy.EfCore.Base
{
    public abstract class Repository<TEntity>: IRepositoryAsync<TEntity> where TEntity: class, IObjectState
    {
        #region Private fields
        private readonly IDataContextAsync _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;
        #endregion Private fields

        [Obsolete("Unit of work doesn't need to be passed to IRepository constructor")]
        public Repository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Repository (IDataContextAsync context)
        {
            _context = context;

            var dbContext = context as Microsoft.EntityFrameworkCore.DbContext;
            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
        }

        public IDataContextAsync Context
        {
            get
            {
                return _context;
            }
        }

        protected virtual TEntity Find(params object[] keyValues)
        {
            //future implementation v1.1.0?
            //return _dbSet.Find(keyValues);
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.FromSql(query, parameters);
        }

        public object ExecuteStoredProcedure(string query, params object[] parameters)
        {
            //https://github.com/aspnet/EntityFramework/issues/245 - 
            var dbContext = _context as DbContext;
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                return cmd.ExecuteNonQuery();
            }
        }

        public async Task<object> ExecuteStoredProcedureAsync(string query, params object[] parameters)
        {
            //https://github.com/aspnet/EntityFramework/issues/245 - 
            var dbContext = _context as DbContext;
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                return await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<System.Data.Common.DbDataReader> ExecuteStoredProcedureReaderAsync(string query, params object[] parameters)
        {
            //https://github.com/aspnet/EntityFramework/issues/245 - 
            var dbContext = _context as DbContext;
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                return await cmd.ExecuteReaderAsync();
            }
        }
        
        public virtual void Insert(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;
            _dbSet.Add(entity);
            _context.SyncObjectState(entity);
        }


        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual void Delete(object id)
        {
            //https://github.com/aspnet/EntityFramework/issues/797
            //var entity = _dbSet.Find(id);
            //Delete(entity);
            throw new NotImplementedException();
        }

        public virtual void Delete(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }
        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            //https://github.com/aspnet/EntityFramework/issues/797
            //return await _dbSet.FindAsync(keyValues);
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            //https://github.com/aspnet/EntityFramework/issues/797
            //return await _dbSet.FindAsync(cancellationToken, keyValues);
            throw new NotImplementedException();
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
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

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        protected virtual void InsertOrUpdateGraph(TEntity entity)
        {
            SyncObjectGraph(entity);
            _entitesChecked = null;
            _dbSet.Attach(entity);
        }

        HashSet<object> _entitesChecked; // tracking of all process entities in the object graph when calling SyncObjectGraph

        protected void SyncObjectGraph(object entity) // scan object graph for all 
        {
            if (_entitesChecked == null)
                _entitesChecked = new HashSet<object>();

            if (_entitesChecked.Contains(entity))
                return;

            _entitesChecked.Add(entity);

            var objectState = entity as IObjectState;

            if (objectState != null && objectState.ObjectState == ObjectState.Added)
                _context.SyncObjectState((IObjectState)entity);

            // Set tracking state for child collections
            foreach (var prop in entity.GetType().GetProperties())
            {
                // Apply changes to 1-1 and M-1 properties
                var trackableRef = prop.GetValue(entity, null) as IObjectState;
                if (trackableRef != null)
                {
                    if (trackableRef.ObjectState == ObjectState.Added)
                        _context.SyncObjectState((IObjectState)entity);

                    SyncObjectGraph(prop.GetValue(entity, null));
                }

                // Apply changes to 1-M properties
                var items = prop.GetValue(entity, null) as IEnumerable<IObjectState>;
                if (items == null) continue;

                Debug.WriteLine("Checking collection: " + prop.Name);

                foreach (var item in items)
                    SyncObjectGraph(item);
            }
        }
    }
}
