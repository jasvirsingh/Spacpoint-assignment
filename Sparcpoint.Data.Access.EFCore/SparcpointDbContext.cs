using Microsoft.EntityFrameworkCore;

namespace Sparcpoint.Data.Access.EFCore
{
    public partial class SparcpointDbContext : DbContext
    {
        public virtual DbSet<ProductEntity> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               // optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=Sparcpoint;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>(entity =>
            {
                //entity.HasKey("Id");
                //entity.ToTable("Customer");

                //entity.Property(e => e.Name).HasColumnName("Name");
                //entity.Property(e => e.Description).HasColumnName("Description");
                //entity.Property(e => e.Description).HasColumnName("Description");

            });
        }

    }
}
