﻿namespace CarDealer.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("Sale")]
    public class SaleDTO
    {
        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }
    }
}
