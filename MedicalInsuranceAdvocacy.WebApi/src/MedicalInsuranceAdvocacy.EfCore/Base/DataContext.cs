using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using MedicalInsuranceAdvocacy.RepoCore.DataContext;
using MedicalInsuranceAdvocacy.RepoCore.Infrastructure;

namespace MedicalInsuranceAdvocacy.EfCore.Base
{
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext, IDataContextAsync
    {
        #region Private Fields
        private readonly Guid _instanceId;
        bool _disposed;
        string userName = string.Empty;
        private IPrincipal principal;

        #endregion Private Fields

        public DataContext(DbContextOptions options): base(options)
        {
            _instanceId = Guid.NewGuid();
        }

        public Guid InstanceId { get { return _instanceId; } }

        public IPrincipal Principal { get { return principal; } set { principal = value; } }

        //TODO: finish writing UserName logic to remove active directory domain from prefix.
        public string UserName
        {
            get
            {
                return principal.Identity.Name;
            }
        }

        Database IDataContextAsync.Database{ get; }

        public override int SaveChanges()
        {
            SyncObjectStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectStatePostCommit();
            return changes;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectStatePreCommit();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectStatePostCommit();
            return changesAsync;
        }

        void IDataContext.SyncObjectState<TEntity>(TEntity entity)
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        private void SyncObjectStatePreCommit()
        {
            foreach(var dbEntityEntry in ChangeTracker.Entries())
            {
                Entry(dbEntityEntry.Entity).State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
            }
        }

        public void SyncObjectStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries().ToList())
            {
                if (dbEntityEntry.State == EntityState.Unchanged)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
                else
                {
                    ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
                }

            }
        }

        public override void Dispose()
        {
            if (!_disposed)
            {

            }

            base.Dispose();
            _disposed = true;
        }
    }
}
