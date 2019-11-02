namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(MyValidator.CustomerLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(MyValidator.EmailLength)]
        public string Email { get; set; }

        [Required]
        public string CreditCardNumber { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}
