using MedicalInsuranceAdvocacy.RepoCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalInsuranceAdvocacy.RepoCore.DataContext
{
    public interface IEntityTypeConfiguration<TEntity> where TEntity: class, IObjectState
    {
        void Map(EntityTypeBuilder<TEntity> entity);
    }
}
