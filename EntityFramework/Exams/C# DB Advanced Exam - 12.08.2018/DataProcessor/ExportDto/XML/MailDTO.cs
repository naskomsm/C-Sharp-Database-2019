namespace SoftJail.DataProcessor.ExportDto.XML
{
    using System.Xml.Serialization;

    [XmlType("Message")]
    public class MailDTO
    {
        public string Description { get; set; }
    }
}
