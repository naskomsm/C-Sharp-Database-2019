namespace TeisterMask.DataProcessor.ImportDto.JSON
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportEmployeesDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Invalid length")]
        [RegularExpression(@"(^[A-Z0-9]+$|^[a-z0-9]+$)")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; }

        public int[] Tasks { get; set; }
    }
}
