namespace SoftJail.DataProcessor.ImportDto.XML
{
    using System.Xml.Serialization;

    [XmlType("Officer")]
    public class OfficersPrisonersDTO
    {
        [XmlElement("Name")]
        public string FullName { get; set; }

        [XmlElement("Money")]
        public decimal Salary { get; set; }

        public string Position { get; set; }

        public string Weapon { get; set; }

        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public PrisonerDTO[] Prisoners { get; set; }
    }
}
