namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validator = Validations.Validator;

    public class Store
    {
        public int StoreId { get; set; }

        [Required]
        [MaxLength(Validator.StoreLength)]
        public string Name { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}
