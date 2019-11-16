namespace Cinema.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class HallImportDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 character in length.")]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Seats { get; set; }
    }
}
