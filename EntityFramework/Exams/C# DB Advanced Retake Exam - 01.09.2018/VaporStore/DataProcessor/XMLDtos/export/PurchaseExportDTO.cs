namespace VaporStore.DataProcessor.XMLDtos.export
{
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class PurchaseExportDTO
    {
        [XmlElement("Card")]
        public string Number { get; set; }

        [XmlElement("Cvc")]
        public string Cvc { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("Game")]
        public GameExportDTO Game { get; set; }
    }
}
