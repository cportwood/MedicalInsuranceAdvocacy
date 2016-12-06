using System;
using MedicalInsuranceAdvocacy.RepoCore.Infrastructure;

namespace MedicalInsuranceAdvocacy.RepoCore.DataContext
{
    public interface IDataContext: IDisposable
    {
        string UserName { get; }
        int SaveChanges();
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;
        void SyncObjectStatePostCommit();
    }
}
