using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalInsuranceAdvocacy.DbContext.DbContexts
{
    public class AspIdentityDbContext : IdentityDbContext
    {
        public AspIdentityDbContext(DbContextOptions<AspIdentityDbContext> options) : base(options) { }
    }
}