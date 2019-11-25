namespace VaporStore.DataProcessor.XMLDtos.import
{
    using System.Xml.Serialization;
    using VaporStore.Data.enums;

    [XmlType("Purchase")]
    public class PurchaseDTO
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement("Type")]
        public PurchaseType Type { get; set; }

        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("Card")]
        public string Number { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }
    }
}
