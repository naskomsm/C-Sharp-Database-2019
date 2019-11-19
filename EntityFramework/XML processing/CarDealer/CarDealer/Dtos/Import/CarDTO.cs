namespace CarDealer.Dtos.Import
{
    using CarDealer.Models;
    using System.Xml.Serialization;
    
    [XmlType("Car")]
    public class CarDTO
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public AnotherPartDTO[] Parts { get; set; }
    }
}
