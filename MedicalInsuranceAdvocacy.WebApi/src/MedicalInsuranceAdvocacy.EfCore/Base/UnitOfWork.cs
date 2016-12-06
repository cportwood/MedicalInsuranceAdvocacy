using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedicalInsuranceAdvocacy.RepoCore.DataContext;
using MedicalInsuranceAdvocacy.RepoCore.UnitOfWork;

namespace MedicalInsuranceAdvocacy.EfCore.Base
{
    public class UnitOfWork: IUnitOfWorkAsync
    {
        private List<IDataContextAsync> _dataContexts = new List<IDataContextAsync>();
        private bool _disposed;

        public UnitOfWork(params IDataContextAsync[] contexts)
        {
            _dataContexts.AddRange(contexts);
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        public void SaveChanges()
        {
            _dataContexts.ForEach(context => context.SaveChanges());
        }

        public async Task<int[]> SaveChangesAsync()
        {
            var tasks = _dataContexts.Select(context => context.SaveChangesAsync());
            var results = await Task.WhenAll(tasks);
            return results;
        }

        public async Task<int[]> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var tasks = _dataContexts.Select(context => context.SaveChangesAsync(cancellationToken));
            var results = await Task.WhenAll(tasks);
            return results;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
