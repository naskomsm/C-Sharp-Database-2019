namespace ProductShop.DTOs
{
    using System.Xml.Serialization;

    [XmlType("SoldProducts")]
    public class SoldProductsExportDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public SoldProductExportDTO[] Products { get; set; }
    }
}
