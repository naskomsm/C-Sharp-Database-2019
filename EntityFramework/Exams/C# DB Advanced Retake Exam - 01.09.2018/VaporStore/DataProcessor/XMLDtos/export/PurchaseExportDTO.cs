namespace VaporStore.DataProcessor.XMLDtos.export
{
    using System;
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class PurchaseExportDTO
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement("Cvc")]
        public string Cvc { get; set; }

        [XmlElement("Date")]
        public DateTime Date { get; set; }

        [XmlElement("Game")]
        public GameExportDTO Game { get; set; }
    }
}
