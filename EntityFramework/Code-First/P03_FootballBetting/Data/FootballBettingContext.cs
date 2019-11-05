namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Models;

    public class FootballBettingContext : DbContext
    {
        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(pc => new { pc.GameId, pc.PlayerId });

            modelBuilder.Entity<Team>(entity =>
            {
                entity
                    .HasOne(t => t.PrimaryKitColor)
                    .WithMany(c => c.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId);

                entity
                    .HasOne(t => t.SecondaryKitColor)
                    .WithMany(c => c.SecondaryKitTeams)
                    .HasForeignKey(t => t.SecondaryKitColorId);

                entity
                    .HasOne(t => t.Town)
                    .WithMany(to => to.Teams)
                    .HasForeignKey(t => t.TownId);

                entity
                    .HasMany(t => t.HomeGames)
                    .WithOne(hg => hg.HomeTeam)
                    .HasForeignKey(hg => hg.HomeTeamId);

                entity
                    .HasMany(t => t.AwayGames)
                    .WithOne(hg => hg.AwayTeam)
                    .HasForeignKey(hg => hg.AwayTeamId);

                entity
                    .HasMany(t => t.Players)
                    .WithOne(p => p.Team)
                    .HasForeignKey(p => p.TeamId);
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity
                    .HasOne(t => t.Country)
                    .WithMany(c => c.Towns)
                    .HasForeignKey(t => t.CountryId);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity
                    .HasOne(pl => pl.Position)
                    .WithMany(po => po.Players)
                    .HasForeignKey(pl => pl.PositionId);

                entity
                    .HasMany(p => p.PlayerStatistics)
                    .WithOne(g => g.Player)
                    .HasForeignKey(g => g.PlayerId);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity
                    .HasMany(g => g.PlayerStatistics)
                    .WithOne(ps => ps.Game)
                    .HasForeignKey(ps => ps.GameId);

                entity
                    .HasMany(g => g.Bets)
                    .WithOne(b => b.Game)
                    .HasForeignKey(b => b.GameId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity
                    .HasMany(u => u.Bets)
                    .WithOne(b => b.User)
                    .HasForeignKey(b => b.UserId);
            });
        }
    }
}
