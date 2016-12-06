using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace MedicalInsuranceAdvocacy.RepoCore.DataContext
{
    public interface IDataContextAsync: IDataContext
    {
        new string UserName { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
        Database Database { get; }
    }
}
