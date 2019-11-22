namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Performer")]
    public class PerformerImputDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid performer first name length")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid performer last name length")]
        public string LastName { get; set; }

        [Required]
        [Range(18, 70)]
        public int Age { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal NetWorth { get; set; }

        [XmlArray("PerformersSongs")]
        public SongIdDTO[] PerformerSongs { get; set; }

    }
}
