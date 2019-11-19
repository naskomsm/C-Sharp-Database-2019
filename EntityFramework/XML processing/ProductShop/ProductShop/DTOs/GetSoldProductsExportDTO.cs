namespace ProductShop.DTOs
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class GetSoldProductsExportDTO
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public SoldProductExportDTO[] ProductsSold { get; set; }
    }
}
