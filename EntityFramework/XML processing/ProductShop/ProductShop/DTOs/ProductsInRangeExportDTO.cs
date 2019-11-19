namespace ProductShop.DTOs
{
    using System.Xml.Serialization;

    [XmlType("Product")]
    public class ProductsInRangeExportDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("buyer")]
        public string Buyer { get; set; }
    }
}
