namespace Cinema.Data.Models
{
    using Cinema.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 20 character in length.")]
        public string Title { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Director must be between 3 and 20 character in length.")]
        public string Director { get; set; }

        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();
    }
}
