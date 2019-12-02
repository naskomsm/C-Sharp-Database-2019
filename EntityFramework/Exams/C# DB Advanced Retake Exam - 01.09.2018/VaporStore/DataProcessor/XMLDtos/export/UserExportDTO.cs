namespace VaporStore.DataProcessor.XMLDtos.export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UserExportDTO
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        public PurchaseExportDTO[] Purchases { get; set; }

        public decimal TotalSpent { get; set; }
    }
}
