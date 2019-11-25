namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class GameTag
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public int TagId { get; set; }

        public Game Game { get; set; }

        public Tag Tag { get; set; }
    }
}
