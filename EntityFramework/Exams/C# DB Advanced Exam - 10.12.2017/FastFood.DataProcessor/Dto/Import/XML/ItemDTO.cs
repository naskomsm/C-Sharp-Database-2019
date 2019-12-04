namespace FastFood.DataProcessor.Dto.Import.XML
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Item")]
    public class ItemDTO
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Name { get; set; }

        [XmlElement("Quantity")]
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
