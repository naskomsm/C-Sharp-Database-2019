namespace SoftJail.DataProcessor.ImportDto.JSON
{
    using SoftJail.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class PrisonersMailsDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression("The [A-Z][a-z]+")]
        public string Nickname { get; set; }

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range(0, double.PositiveInfinity)]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public ICollection<Mail> Mails { get; set; }
    }
}
