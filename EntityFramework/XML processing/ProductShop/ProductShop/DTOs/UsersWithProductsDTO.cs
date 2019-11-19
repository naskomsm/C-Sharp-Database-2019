namespace ProductShop.Dtos
{
    using ProductShop.DTOs;
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UsersWithProductsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public SoldProductsExportDTO ProductsSold { get; set; }
    }
}
