using System.Threading.Tasks;
using MedicalInsuranceAdvocacy.RepoCore.Infrastructure;

namespace MedicalInsuranceAdvocacy.RepoCore.Interfaces
{
    public interface IRepositoryAsync<TEntity>: IRepository<TEntity> where TEntity: class, IObjectState
    {
        Task<object> ExecuteStoredProcedureAsync(string query, params object[] parameters);
    }
}
