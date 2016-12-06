using System.Threading;
using System.Threading.Tasks;

namespace MedicalInsuranceAdvocacy.RepoCore.UnitOfWork
{
    public interface IUnitOfWorkAsync: IUnitOfWork
    {
        Task<int[]> SaveChangesAsync();
        Task<int[]> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
