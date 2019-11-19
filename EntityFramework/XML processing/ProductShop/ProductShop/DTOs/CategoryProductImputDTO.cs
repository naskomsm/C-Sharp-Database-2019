namespace ProductShop.DTOs
{
    using ProductShop.Models;
    using System.Xml.Serialization;

    [XmlType("CategoryProduct")]
    public class CategoryProductImputDTO
    {
        public int CategoryId { get; set; }

        public int ProductId { get; set; }
    }
}
