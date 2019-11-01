namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validator = Validations.Validator;

    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(Validator.ProductLength)]
        public string Name { get; set; }

        [MaxLength(Validator.Description)]
        public string Description { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}
