using CafeEposAPI.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CafeEposAPI.Data
{
    public class EposDbContext : DbContext
    {
        public EposDbContext(DbContextOptions<EposDbContext> Options) : base(Options)
        {

        }

        public DbSet<SystemAccountEntity> SystemAccounts { get; set; }
        public DbSet<staffLoginEntity> StaffAccounts { get; set; }
        public DbSet<menuEntity> Menu { get; set; }
        public DbSet<categoryEntity> Category { get; set; }
    }
}
