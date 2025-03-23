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
        public DbSet<OrderInfoEntity> OrderInfo { get; set; }
        public DbSet<OrderItemsEntity> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<menuEntity>()
                .HasOne(e => e.category)
                .WithMany(e => e.menuItems)
                .HasForeignKey(e => e.categortyId);

            modelBuilder.Entity<categoryEntity>()
                .HasOne(e => e.parentCategory)
                .WithMany()
                .HasForeignKey(e => e.parentId);
        }
    }
}
