namespace P01_StudentSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Resource
    {
        public int ResourceId { get; set; }

        [Required]
        [MaxLength(MyValidator.ResourceNameLength)]
        public string Name { get; set; }

        public string Url { get; set; }

        [Required]
        public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
