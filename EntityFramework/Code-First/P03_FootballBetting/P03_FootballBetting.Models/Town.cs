namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using P03_FootballBetting.Configurations;
    using System.ComponentModel.DataAnnotations;

    public class Town
    {
        public int TownId { get; set; }

        [Required]
        [MaxLength(MyValidator.Town.NameLength)]
        public string Name { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    }
}
