namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProducerImportDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid producer name length")]
        public string Name { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }

        [RegularExpression(@"\+359 [0-9]{3} [0-9]{3} [0-9]{3}")]
        public string PhoneNumber { get; set; }

        public ICollection<AlbumImportDTO> Albums { get; set; } = new HashSet<AlbumImportDTO>();
    }
}
