namespace FastFood.DataProcessor.Dto.Import.XML
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Order")]
    public class ImportOrdersDTO
    {
        [XmlElement("Customer")]
        [Required]
        public string Customer { get; set; }

        [XmlElement("Employee")]
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid length")]
        public string Employee { get; set; }

        [XmlElement("DateTime")]
        [Required]
        public string DateTime { get; set; }

        [XmlElement("Type")]
        [Required]
        public string Type { get; set; }

        [XmlArray("Items")]
        public ItemDTO[] Items{ get; set; }
    }
}
