using Microsoft.EntityFrameworkCore;

namespace Sparcpoint.Data.Access.EF
{
    public partial class SparcpointDbContext : DbContext
    {
        public virtual DbSet<ProductEntity> Products { get; set; }
        
        public virtual DbSet<CategoryEntity> Categories { get; set; }

        public virtual DbSet<ProductCategoryEntity> ProductCategories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=Sparcpoint;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.HasKey("InstanceId");
                entity.ToTable("Products");

                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.ProductImageUris).HasColumnName("ImageUris");
                entity.Property(e => e.Description).HasColumnName("ValidSkus");
            });

            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.HasKey("InstanceId");
                entity.ToTable("Categories");

                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Description).HasColumnName("Description");
            });

            modelBuilder.Entity<ProductCategoryEntity>(entity =>
            {
                entity.ToTable("ProductCategories");

                entity.Property(e => e.InstanceId).HasColumnName("InstanceId");
                entity.Property(e => e.CategoryInstanceId).HasColumnName("CategoryInstanceId");
            });
        }

    }
}
