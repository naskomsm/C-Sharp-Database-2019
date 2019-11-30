namespace VaporStore.DataProcessor.XMLDtos.export
{
    using System.Xml.Serialization;
    using VaporStore.Data.Models;

    [XmlType("Game")]
    public class GameExportDTO
    {
        [XmlAttribute("title")]
        public string Name { get; set; }

        [XmlElement("Genre")]
        public Genre Genre { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
