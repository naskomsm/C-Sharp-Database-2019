namespace SoftJail.DataProcessor.ExportDto.XML
{
    using System.Xml.Serialization;

    [XmlType("Prisoner")]
    public class PrisonersInboxDTO
    {
        public int Id { get; set; }

        [XmlElement("Name")]
        public string FullName { get; set; }

        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public MailDTO[] Mails { get; set; }
    }
}
