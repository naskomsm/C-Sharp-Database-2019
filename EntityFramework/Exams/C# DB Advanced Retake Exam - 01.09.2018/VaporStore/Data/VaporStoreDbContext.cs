namespace VaporStore.Data
{
	using Microsoft.EntityFrameworkCore;
    using VaporStore.Data.Models;

    public class VaporStoreDbContext : DbContext
	{
		public VaporStoreDbContext()
		{
		}

		public VaporStoreDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Card> Cards { get; set; }

		public DbSet<Developer> Developers { get; set; }

		public DbSet<Game> Games { get; set; }

		public DbSet<GameTag> GameTags { get; set; }

		public DbSet<Genre> Genres { get; set; }

		public DbSet<Purchase> Purchases { get; set; }

		public DbSet<Tag> Tags { get; set; }

		public DbSet<User> Users { get; set; }


		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (!options.IsConfigured)
			{
				options
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder model)
		{
			model.Entity<GameTag>()
				.HasKey(key => new { key.GameId, key.TagId });

			model.Entity<GameTag>()
				.HasOne(gt => gt.Game)
				.WithMany(g => g.GameTags)
				.HasForeignKey(gt => gt.GameId);

			model.Entity<GameTag>()
				.HasOne(gt => gt.Tag)
				.WithMany(t => t.GameTags)
				.HasForeignKey(gt => gt.TagId);

			model.Entity<User>()
				.HasMany(u => u.Cards)
				.WithOne(c => c.User)
				.HasForeignKey(c => c.UserId);

			model.Entity<Card>()
				.HasMany(c => c.Purchases)
				.WithOne(p => p.Card)
				.HasForeignKey(p => p.CardId);
		}
	}
}