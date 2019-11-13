namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "FirstName must be between 3 and 20 character in length.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "LastName must be between 3 and 20 character in length.")]
        public string LastName { get; set; }

        [Required]
        [Range(12, 110)]
        public int Age { get; set; }

        [Required]
        [Range(0.01, Double.PositiveInfinity)] // could be problem
        public decimal Balance { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
