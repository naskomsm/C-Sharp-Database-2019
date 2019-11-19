namespace ProductShop.DTOs
{
    using System.Xml.Serialization;

    [XmlType("Product")]
    public class SoldProductExportDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
