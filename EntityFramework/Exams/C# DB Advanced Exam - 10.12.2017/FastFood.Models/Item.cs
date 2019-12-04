namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Item
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        [Range(0.01, double.PositiveInfinity)]
        public decimal Price { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
