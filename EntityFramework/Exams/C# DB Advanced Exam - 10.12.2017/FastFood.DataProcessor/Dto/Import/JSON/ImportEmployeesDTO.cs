namespace FastFood.DataProcessor.Dto.Import.JSON
{
    using System.ComponentModel.DataAnnotations;

    public class ImportEmployeesDTO
    {
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Name { get; set; }

        [Range(15, 80)]
        public int Age { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Position { get; set; }
    }
}
