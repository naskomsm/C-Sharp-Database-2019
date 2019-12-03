﻿namespace SoftJail.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Name { get; set; }

        public ICollection<Cell> Cells { get; set; } = new HashSet<Cell>();
    }
}
