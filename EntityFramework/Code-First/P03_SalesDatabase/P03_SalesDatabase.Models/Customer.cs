namespace P03_SalesDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Customer
    {
        public int CustomerId { get; set; }

        [MaxLength(MyValidator.CustomerNameLength)]
        public string Name { get; set; }

        [MaxLength(MyValidator.CustomerEmailLength)]
        public string Email { get; set; }

        public string CreditCardNumber { get; set; }

        public ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}
