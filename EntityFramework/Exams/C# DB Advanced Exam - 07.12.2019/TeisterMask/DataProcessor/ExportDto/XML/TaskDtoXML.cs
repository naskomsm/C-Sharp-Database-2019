namespace TeisterMask.DataProcessor.ExportDto.XML
{
    using System.Xml.Serialization;

    [XmlType("Task")]
    public class TaskDtoXML
    {
        public string Name { get; set; }

        public string Label { get; set; }
    }
}
