namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validator = Validations.Validator;

    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(Validator.CustomerLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(Validator.EmailLength)]
        public string Email { get; set; }

        [Required]
        public string CreditCardNumber { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}
