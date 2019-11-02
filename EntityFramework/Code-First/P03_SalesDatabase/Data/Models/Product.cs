namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(MyValidator.ProductLength)]
        public string Name { get; set; }

        [MaxLength(MyValidator.Description)]
        public string Description { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}
