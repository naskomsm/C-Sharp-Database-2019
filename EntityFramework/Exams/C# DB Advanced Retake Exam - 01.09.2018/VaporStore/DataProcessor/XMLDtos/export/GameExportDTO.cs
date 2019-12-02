namespace VaporStore.DataProcessor.XMLDtos.export
{
    using System.Xml.Serialization;

    [XmlType("Game")]
    public class GameExportDTO
    {
        [XmlAttribute("title")]
        public string Name { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }
    }
}
