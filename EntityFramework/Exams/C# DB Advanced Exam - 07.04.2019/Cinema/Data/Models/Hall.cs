﻿namespace Cinema.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Hall
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 character in length.")]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();

        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
    }
}
