namespace FastFood.Data
{
	using Models;
	using Microsoft.EntityFrameworkCore;

	public class FastFoodDbContext : DbContext
	{
		public FastFoodDbContext()
		{
		}

		public FastFoodDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Category> Categories { get; set; }

		public DbSet<Employee> Employees { get; set; }

		public DbSet<Item> Items { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderItem> OrderItems { get; set; }

		public DbSet<Position> Positions { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			if (!builder.IsConfigured)
			{
				builder.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<OrderItem>(entity =>
			{
				entity.HasKey(k => new { k.ItemId, k.OrderId });

				entity
					.HasOne(oi => oi.Order)
					.WithMany(o => o.OrderItems)
					.HasForeignKey(oi => oi.OrderId);

				entity
					.HasOne(oi => oi.Item)
					.WithMany(i => i.OrderItems)
					.HasForeignKey(oi => oi.ItemId);
			});

			builder
				.Entity<Category>()
				.HasMany(c => c.Items)
				.WithOne(i => i.Category)
				.HasForeignKey(i => i.CategoryId);

			builder
				.Entity<Employee>()
				.HasOne(e => e.Position)
				.WithMany(p => p.Employees)
				.HasForeignKey(e => e.PositionId);

			builder
				.Entity<Employee>()
				.HasMany(e => e.Orders)
				.WithOne(o => o.Employee)
				.HasForeignKey(o => o.EmployeeId);
		}
	}
}