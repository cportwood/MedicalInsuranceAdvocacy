using MedicalInsuranceAdvocacy.DataModel.Patient;
using MedicalInsuranceAdvocacy.RepoCore.DataContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalInsuranceAdvocacy.DbContext.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Map(EntityTypeBuilder<Address> entity)
        {
            entity.HasOne(e => e.Patient);


        }
    }
}
