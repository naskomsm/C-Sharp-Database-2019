namespace CarDealer
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CarDealer.Data;
    using CarDealer.Dtos.Export;
    using CarDealer.Dtos.Import;
    using CarDealer.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            context.Database.Migrate();

            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            var suppliersXml = File.ReadAllText(@"C:\Users\pc1\Desktop\code\C-Sharp-Database-2019\EntityFramework\XML processing\CarDealer\CarDealer\Datasets\suppliers.xml");
            var partsXml = File.ReadAllText(@"C:\Users\pc1\Desktop\code\C-Sharp-Database-2019\EntityFramework\XML processing\CarDealer\CarDealer\Datasets\parts.xml");
            var carsXml = File.ReadAllText(@"C:\Users\pc1\Desktop\code\C-Sharp-Database-2019\EntityFramework\XML processing\CarDealer\CarDealer\Datasets\cars.xml");
            var customersXml = File.ReadAllText(@"C:\Users\pc1\Desktop\code\C-Sharp-Database-2019\EntityFramework\XML processing\CarDealer\CarDealer\Datasets\customers.xml");
            var salesXml = File.ReadAllText(@"C:\Users\pc1\Desktop\code\C-Sharp-Database-2019\EntityFramework\XML processing\CarDealer\CarDealer\Datasets\sales.xml");

            using (context)
            {
                //Console.WriteLine(ImportSuppliers(context, suppliersXml));
                //Console.WriteLine(ImportParts(context, partsXml));
                //Console.WriteLine(ImportCars(context, carsXml));
                //Console.WriteLine(ImportCustomers(context, customersXml));
                //Console.WriteLine(ImportSales(context, salesXml));

                Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SupplierDTO>), new XmlRootAttribute("Suppliers"));
            var suppliersDTOs = (List<SupplierDTO>)serializer.Deserialize(new StringReader(inputXml));

            var suppliers = Mapper.Map<List<SupplierDTO>, List<Supplier>>(suppliersDTOs);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<PartDTO>), new XmlRootAttribute("Parts"));
            var partsDTOs = (List<PartDTO>)serializer.Deserialize(new StringReader(inputXml));

            var parts = Mapper.Map<List<PartDTO>, List<Part>>(partsDTOs);

            var suppliersIds = context
                .Suppliers
                .Select(s => s.Id)
                .ToList();

            var validParts = parts
                .Where(p => suppliersIds.Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}"; ;
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CarDTO>), new XmlRootAttribute("Cars"));
            var carsDTOs = (List<CarDTO>)serializer.Deserialize(new StringReader(inputXml));

            var cars = new List<Car>();
            var partCars = new List<PartCar>();

            foreach (var carDTO in carsDTOs)
            {
                var car = new Car()
                {
                    Make = carDTO.Make,
                    Model = carDTO.Model,
                    TravelledDistance = carDTO.TravelledDistance
                };

                var parts = carDTO.Parts
                    .Where(p => context.Parts.Any(x => x.Id == p.Id))
                    .Select(p => p.Id)
                    .Distinct()
                    .ToList();

                foreach (var partId in parts)
                {
                    var partCar = new PartCar()
                    {
                        PartId = partId,
                        Car = car
                    };

                    partCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(partCars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CustomerDTO>), new XmlRootAttribute("Customers"));
            var customersDTOs = (List<CustomerDTO>)serializer.Deserialize(new StringReader(inputXml));

            var customers = Mapper.Map<List<CustomerDTO>, List<Customer>>(customersDTOs);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SaleDTO>), new XmlRootAttribute("Sales"));
            var salesDTOs = (List<SaleDTO>)serializer.Deserialize(new StringReader(inputXml));

            var sales = new List<Sale>();

            var carIds = context
                .Cars
                .Select(c => c.Id)
                .ToList();

            foreach (var saleDTO in salesDTOs)
            {
                if (!carIds.Contains(saleDTO.CarId))
                {
                    continue;
                }

                var sale = new Sale()
                {
                    CarId = saleDTO.CarId,
                    CustomerId = saleDTO.CustomerId,
                    Discount = saleDTO.Discount
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.TravelledDistance > 2000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<CarWithDistanceExportDTO>()
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<CarWithDistanceExportDTO>), new XmlRootAttribute("cars"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), cars, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<CarBMWExportDTO>()
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<CarBMWExportDTO>), new XmlRootAttribute("cars"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), cars, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(s => !s.IsImporter)
                .ProjectTo<SupplierExportDTO>()
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<SupplierExportDTO>), new XmlRootAttribute("suppliers"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), suppliers, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new CarsWithPartsExportDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars
                    .Select(pc => new PartExportDTO
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<CarsWithPartsExportDTO>), new XmlRootAttribute("cars"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), cars, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Where(c => c.Sales.Any(s => s.CustomerId == c.Id))
                .Select(c => new CustomerExportDTO
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Select(s => s.CarId).Count(),
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<CustomerExportDTO>), new XmlRootAttribute("customers"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), customers, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Select(s => new SaleExportDTO
                {
                    Car = new CarWithDistanceExportDTO
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) - (s.Car.PartCars.Sum(pc => pc.Part.Price) * (s.Discount/100))
                })
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<SaleExportDTO>), new XmlRootAttribute("sales"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), sales, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}