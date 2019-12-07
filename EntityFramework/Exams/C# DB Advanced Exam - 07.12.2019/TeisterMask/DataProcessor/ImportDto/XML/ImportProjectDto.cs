namespace TeisterMask.DataProcessor.ImportDto.XML
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ImportProjectDto
    {
        [Required]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Invalid length")]
        public string Name { get; set; }

        [Required]
        public string OpenDate { get; set; }

        public TaskXMLDto[] Tasks { get; set; }
    }
}
