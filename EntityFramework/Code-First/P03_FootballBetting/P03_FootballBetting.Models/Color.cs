namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using P03_FootballBetting.Configurations;
    using System.ComponentModel.DataAnnotations;

    public class Color
    {
        public int ColorId { get; set; }

        [Required]
        [MaxLength(MyValidator.Color.NameLength)]
        public string Name { get; set; }

        public ICollection<Team> PrimaryKitTeams { get; set; } = new HashSet<Team>();

        public ICollection<Team> SecondaryKitTeams { get; set; } = new HashSet<Team>();
    }
}
