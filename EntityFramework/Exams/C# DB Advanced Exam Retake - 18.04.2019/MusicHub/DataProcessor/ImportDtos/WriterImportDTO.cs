namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class WriterImportDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid writer name length")]
        public string Name { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }
    }
}
