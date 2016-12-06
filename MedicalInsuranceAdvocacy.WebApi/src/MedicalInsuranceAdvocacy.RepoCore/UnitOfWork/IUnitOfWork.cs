using System;

namespace MedicalInsuranceAdvocacy.RepoCore.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        void SaveChanges();
        void Dispose(bool disposing);
    }
}
