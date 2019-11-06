namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Position
    {
        public int PositionId { get; set; }
        
        [Required]
        [MaxLength(MyValidator.Position.NameLength)]
        public string Name { get; set; }

        public ICollection<Player> Players { get; set; } = new HashSet<Player>();
    }
}
