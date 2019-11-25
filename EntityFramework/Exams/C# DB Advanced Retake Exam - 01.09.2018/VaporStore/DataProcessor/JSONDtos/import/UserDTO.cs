namespace VaporStore.DataProcessor.JSONDtos.import
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserDTO
    {
        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string FullName { get; set; }

        public string Email { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Username { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        public ICollection<CardDTO> Cards { get; set; }
    }
}
