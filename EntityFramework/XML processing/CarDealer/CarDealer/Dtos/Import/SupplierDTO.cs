namespace CarDealer.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("Supplier")]
    public class SupplierDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("isimporter")]
        public bool IsImporter { get; set; }
    }
}
