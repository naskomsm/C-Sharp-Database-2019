namespace VaporStore.DataProcessor.XMLDtos.import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using VaporStore.Data.enums;
    using VaporStore.Data.Models;

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
        [RegularExpression(@"[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}")]
        public string CardNumber { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }
    }
}
