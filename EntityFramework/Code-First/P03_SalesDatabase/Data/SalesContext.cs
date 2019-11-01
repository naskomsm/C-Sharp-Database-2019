namespace P03_SalesDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_SalesDatabase.Data.Models;

    public class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions options)
            : base(options)
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Sale> Sales { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(c => c.Email)
                .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasMany(p => p.Sales)
                    .WithOne(s => s.Product)
                    .HasForeignKey(s => s.ProductId);

                entity.Property(p => p.Description)
                .HasDefaultValue("No description");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.Date)
                .HasDefaultValueSql("GETDATE()"); //not working
                
                entity
                .HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId);

                entity
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId);

                entity
                .HasOne(s => s.Store)
                .WithMany(st => st.Sales)
                .HasForeignKey(s => s.StoreId);
            });
        }
    }
}
