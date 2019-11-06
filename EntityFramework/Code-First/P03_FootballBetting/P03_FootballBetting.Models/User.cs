namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using P03_FootballBetting.Configurations;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(MyValidator.User.UsernameLength)]
        public string Username { get; set; }

        [Required]
        [MaxLength(MyValidator.User.PasswordLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(MyValidator.User.EmailLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(MyValidator.User.NameLength)]
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public ICollection<Bet> Bets { get; set; } = new HashSet<Bet>();
    }
}
