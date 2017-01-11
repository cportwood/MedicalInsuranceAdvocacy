using MedicalInsuranceAdvocacy.RepoCore.DataContext;
using MedicalInsuranceAdvocacy.RepoCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MedicalInsuranceAdvocacy.DbContext
{
    public static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, IEntityTypeConfiguration<TEntity> configuration) where TEntity: class, IObjectState
        {
            configuration.Map(modelBuilder.Entity<TEntity>());
        }
    }
}
