namespace FastFood.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using FastFood.Data;
    using FastFood.DataProcessor.Dto.Export.JSON;
    using FastFood.DataProcessor.Dto.Export.XML;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
	{
		public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
		{
            var result = context.Employees
               .Where(e => e.Name == employeeName)
               .Select(e => new ExportEmployeeWithOrdersDto
               {
                   Name = e.Name,
                   Orders = e.Orders
                   .Where(o => o.Type.ToString() == orderType)
                   .Select(o => new ExportOrdersByEmployeeDto
                   {
                       Customer = o.Customer,
                       Items = o.OrderItems.Select(oi => new ExportItemsForOrderDto
                       {
                           Name = oi.Item.Name,
                           Price = oi.Item.Price,
                           Quantity = oi.Quantity
                       })
                       .ToList(),
                       TotalPrice = o.TotalPrice
                   })
                   .OrderByDescending(o => o.TotalPrice)
                   .ThenByDescending(o => o.Items.Count)
                   .ToList(),
                   TotalMade = e.Orders.Sum(p => p.TotalPrice)
               })
               .ToList();

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

		public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
		{
            var xmlSerializer = new XmlSerializer(typeof(List<CategoryDTO>), new XmlRootAttribute("Categories"));
            var sb = new StringBuilder();

            var categoriesNames = categoriesString.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

            var categories = context
                .Categories
                .Where(c => categoriesNames.Contains(c.Name))
                .Select(c => new CategoryDTO
                {
                    Name = c.Name,
                    MostPopularItem = c.Items.Select(i => new ItemDTO
                    {
                        Name = i.Name,
                        TotalMade = i.Price * i.OrderItems.Sum(oi => oi.Quantity),
                        TimesSold = i.OrderItems.Sum(oi => oi.Quantity)
                    })
                    .OrderByDescending(i => i.TotalMade)
                    .FirstOrDefault()
                })
                .OrderByDescending(i => i.MostPopularItem.TotalMade)
                .ThenByDescending(i => i.MostPopularItem.TimesSold)
                .ToList();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().TrimEnd();
		}
	}
}