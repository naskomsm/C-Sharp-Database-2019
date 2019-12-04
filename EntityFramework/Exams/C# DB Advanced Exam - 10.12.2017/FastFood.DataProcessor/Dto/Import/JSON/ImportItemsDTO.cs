namespace FastFood.DataProcessor.Dto.Import.JSON
{
    using System.ComponentModel.DataAnnotations;

    public class ImportItemsDTO
    {
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Name { get; set; }

        [Range(0.01, double.PositiveInfinity)]
        public decimal Price { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Category { get; set; }
    }
}
