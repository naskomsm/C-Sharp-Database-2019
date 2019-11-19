namespace ProductShop.DTOs
{
    using System.Xml.Serialization;

    [XmlType("Category")]
    public class CategoryImputDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
