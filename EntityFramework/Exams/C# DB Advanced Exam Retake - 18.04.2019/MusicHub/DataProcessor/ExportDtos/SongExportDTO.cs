namespace MusicHub.DataProcessor.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class SongExportDTO
    {
        [XmlElement("SongName")]
        public string Name { get; set; }

        [XmlElement("Writer")]
        public string WriterName { get; set; }

        [XmlElement("Performer")]
        public string PerformerName { get; set; }

        [XmlElement("AlbumProducer")]
        public string AlbumProducerName { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }
    }
}
