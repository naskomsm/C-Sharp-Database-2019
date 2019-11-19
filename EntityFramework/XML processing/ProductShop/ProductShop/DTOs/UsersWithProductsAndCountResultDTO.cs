namespace ProductShop.Dtos
{
    using System.Xml.Serialization;

    [XmlType("Users")]
    public class UsersWithProductsAndCountResultDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UsersWithProductsDto[] Users { get; set; }
    }
}
