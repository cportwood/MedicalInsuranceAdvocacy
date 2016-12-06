using MedicalInsuranceAdvocacy.DataModel.Patient;
using MedicalInsuranceAdvocacy.EfCore;
using MedicalInsuranceAdvocacy.EfCore.Base;
using Microsoft.EntityFrameworkCore;
using AddressConfiguration = MedicalInsuranceAdvocacy.DbContext.Configurations.AddressConfiguration;

namespace MedicalInsuranceAdvocacy.DbContext.DbContexts
{
    public class ApplicationContext:DataContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new AddressConfiguration());
        }
    }
}
