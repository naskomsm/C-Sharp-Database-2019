namespace TeisterMask.DataProcessor.ExportDto.XML
{
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ProjectDto
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TaskDtoXML[] Tasks { get; set; }
    }
}
