namespace FastFood.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using FastFood.DataProcessor.Dto.Import.JSON;
    using FastFood.DataProcessor.Dto.Import.XML;
    using FastFood.Models.Enums;
    using Models;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            var employeesImport = JsonConvert.DeserializeObject<List<ImportEmployeesDTO>>(jsonString);

            var employees = new List<Employee>();
            var positions = new List<Position>();

            var sb = new StringBuilder();

            foreach (var dto in employeesImport)
            {
                var isEmployeeValid = IsValid(dto);

                if (!isEmployeeValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var position = positions.FirstOrDefault(p => p.Name == dto.Position);

                if (position == null)
                {
                    position = new Position
                    {
                        Name = dto.Position
                    };

                    positions.Add(position);
                }

                var employee = new Employee()
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    Position = position
                };

                sb.AppendLine(string.Format(SuccessMessage, employee.Name));
                employees.Add(employee);
            }

            context.Employees.AddRange(employees);
            context.Positions.AddRange(positions);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var itemsDTOs = JsonConvert.DeserializeObject<List<ImportItemsDTO>>(jsonString);

            var sb = new StringBuilder();

            var items = new List<Item>();
            var categories = new List<Category>();

            foreach (var dto in itemsDTOs)
            {
                var isItemValid = IsValid(dto);

                if (!isItemValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (items.Any(i => i.Name == dto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var category = categories.FirstOrDefault(p => p.Name == dto.Category);

                if (category == null)
                {
                    category = new Category
                    {
                        Name = dto.Category
                    };

                    categories.Add(category);
                }

                var item = new Item()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Category = category
                };

                sb.AppendLine(string.Format(SuccessMessage, item.Name));
                items.Add(item);
            }

            context.Items.AddRange(items);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ImportOrdersDTO>), new XmlRootAttribute("Orders"));
            var ordersDTOs = (List<ImportOrdersDTO>)serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var orders = new List<Order>();
            var orderItems = new List<OrderItem>();

            var dbEmployees = context
                .Employees
                .Select(e => e.Name)
                .ToList();

            var dbItems = context
                .Items
                .Select(i => i.Name)
                .ToList();

            foreach (var dto in ordersDTOs)
            {
                var isValidDto = IsValid(dto);
                var employee = context.Employees.FirstOrDefault(e => e.Name == dto.Employee);
                var itemsNames = context.Items.Select(i => i.Name).ToList();
                var isValidItems = dto.Items.Select(i => i.Name).All(v => itemsNames.Contains(v));
                var isValidOrderType = Enum.IsDefined(typeof(OrderType), dto.Type);

                if (isValidDto == false ||
                    employee == null ||
                    isValidItems == false ||
                    isValidOrderType == false)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var order = new Order()
                {
                    Customer = dto.Customer,
                    Employee = context.Employees.FirstOrDefault(x => x.Name == dto.Employee),
                    DateTime = DateTime.ParseExact(dto.DateTime, @"dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Type = (OrderType)Enum.Parse(typeof(OrderType), dto.Type, true) 
                };

                var isOkToAdd = true;

                foreach (var itemDto in dto.Items)
                {
                    if (!dbItems.Contains(itemDto.Name))
                    {
                        sb.AppendLine(FailureMessage);
                        isOkToAdd = false;
                        break;
                    }

                    var item = context.Items.FirstOrDefault(x => x.Name == itemDto.Name);

                    var orderItem = new OrderItem()
                    {
                        Item = item,
                        Quantity = itemDto.Quantity
                    };

                    orderItems.Add(orderItem);
                    order.OrderItems.Add(orderItem);
                }

                if (isOkToAdd)
                {
                    orders.Add(order);
                    sb.AppendLine($"Order for {order.Customer} on {order.DateTime.ToString(@"dd/MM/yyyy HH:mm")} added");
                }
            }

            context.Orders.AddRange(orders);
            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}