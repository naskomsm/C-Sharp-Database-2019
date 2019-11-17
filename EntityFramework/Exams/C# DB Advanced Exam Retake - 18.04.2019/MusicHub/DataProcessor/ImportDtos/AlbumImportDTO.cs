namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class AlbumImportDTO
    {
        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Invalid album name length")]
        public string Name { get; set; }

        [Required]
        public string ReleaseDate { get; set; }
    }
}
