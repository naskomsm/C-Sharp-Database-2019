namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.Dtos.Import;
    using CarDealer.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            context.Database.Migrate();

            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            var suppliersXml = File.ReadAllText(@"C:\Users\pc1\Downloads\CarDealer - Skeleton\CarDealer\Datasets\suppliers.xml");
            var partsXml = File.ReadAllText(@"C:\Users\pc1\Downloads\CarDealer - Skeleton\CarDealer\Datasets\parts.xml");
            var carsXml = File.ReadAllText(@"C:\Users\pc1\Downloads\CarDealer - Skeleton\CarDealer\Datasets\cars.xml");
            var customersXml = File.ReadAllText(@"C:\Users\pc1\Downloads\CarDealer - Skeleton\CarDealer\Datasets\customers.xml");

            using (context)
            {
                Console.WriteLine(ImportCustomers(context, customersXml));
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
    }
}