namespace VaporStore.DataProcessor.JSONDtos.import
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class GamesDTO
    {
        public string Name { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal Price { get; set; }

        public string ReleaseDate { get; set; }

        public string Developer { get; set; }

        public string Genre { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}
