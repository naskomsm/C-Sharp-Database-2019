namespace ProductShop.DTOs
{
    using System.Xml.Serialization;

    [XmlType("Category")]
    public class CategoriesByProductsExportDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("count")]
        public int NumberOfProducts { get; set; }

        [XmlElement("averagePrice")]
        public decimal AveragePriceProducts { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalPriceSum { get; set; }
    }
}
