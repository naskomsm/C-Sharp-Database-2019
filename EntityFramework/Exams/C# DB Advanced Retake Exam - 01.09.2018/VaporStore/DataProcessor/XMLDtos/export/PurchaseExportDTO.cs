namespace VaporStore.DataProcessor.XMLDtos.export
{
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class PurchaseExportDTO
    {
        public string Card { get; set; }

        public string Cvc { get; set; }

        public string Date { get; set; }

        public GameExportDTO Game { get; set; }
    }
}
