namespace FastFood.DataProcessor.Dto.Export.XML
{
    using System.Xml.Serialization;

    [XmlType("Category")]
    public class CategoryDTO
    {
        public string Name { get; set; }

        public ItemDTO MostPopularItem { get; set; }
    }
}
