namespace ProductShop.DTOs
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UserImputDTO
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int Age { get; set; }
    }
}
