﻿namespace ProductShop.DTOs
{
    using System.Xml.Serialization;

    [XmlType("Product")]
    public class ProductImputDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("sellerId")]
        public int SellerId { get; set; }

        [XmlElement("buyerId")]
        public int? BuyerId { get; set; }
    }
}
