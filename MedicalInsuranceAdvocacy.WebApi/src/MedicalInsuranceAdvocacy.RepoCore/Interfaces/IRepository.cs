using System.Linq;
using MedicalInsuranceAdvocacy.RepoCore.DataContext;
using MedicalInsuranceAdvocacy.RepoCore.Infrastructure;

namespace MedicalInsuranceAdvocacy.RepoCore.Interfaces
{
    public interface IRepository<TEntity> where TEntity: class, IObjectState
    {
        IDataContextAsync Context { get; }
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        object ExecuteStoredProcedure(string query, params object[] parameters);

        IQueryable<TEntity> Queryable();
    }
}
