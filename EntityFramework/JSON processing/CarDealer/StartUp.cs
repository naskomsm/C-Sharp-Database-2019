namespace CarDealer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.DTO;
    using CarDealer.Models;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class StartUp
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings() 
        { 
            Formatting = Formatting.Indented
        };

        public static void Main()
        {
            var dbContext = new CarDealerContext();

            var jsonSuppliers = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Car Dealer\CarDealer\Datasets\suppliers.json");
            var jsonParts = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Car Dealer\CarDealer\Datasets\parts.json");
            var jsonCars = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Car Dealer\CarDealer\Datasets\cars.json");
            var jsonCustomers = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Car Dealer\CarDealer\Datasets\customers.json");
            var jsonSales = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Car Dealer\CarDealer\Datasets\sales.json");

            dbContext.Database.Migrate();

            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            using (dbContext)
            {
                // Seed
                Console.WriteLine(ImportSuppliers(dbContext, jsonSuppliers));
                Console.WriteLine(ImportParts(dbContext, jsonParts));
                Console.WriteLine(ImportCars(dbContext, jsonCars));
                Console.WriteLine(ImportCustomers(dbContext, jsonCustomers));
                Console.WriteLine(ImportSales(dbContext, jsonSales));

                // Queries
                Console.WriteLine(GetSalesWithAppliedDiscount(dbContext));
            }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var suppliersId = context
                .Suppliers
                .Select(s => s.Id)
                .ToList();

            var validParts = new List<Part>();

            foreach (var part in parts)
            {
                if (suppliersId.Contains(part.SupplierId))
                {
                    validParts.Add(part);
                }
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonConvert.DeserializeObject<List<CarImportDTO>>(inputJson);
            var carsToAdd = Mapper.Map<List<CarImportDTO>, List<Car>>(cars);

            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            var partIds = context
                .Parts
                .Select(p => p.Id)
                .ToHashSet();

            var carPartsToAdd = new HashSet<PartCar>();

            foreach (var car in cars)
            {
                car.PartsId = car
                    .PartsId
                    .Distinct()
                    .ToList();

                var currentCar = context
                    .Cars
                    .FirstOrDefault(c => c.Make == car.Make
                                    && c.Model == car.Model
                                    && c.TravelledDistance == car.TravelledDistance);

                if (currentCar == null)
                {
                    continue;
                }

                foreach (var partId in car.PartsId)
                {
                    if (!partIds.Contains(partId))
                    {
                        continue; // this part does not exist
                    }

                    var partCar = new PartCar
                    {
                        CarId = currentCar.Id,
                        PartId = partId
                    };

                    if (!carPartsToAdd.Contains(partCar))
                    {
                        carPartsToAdd.Add(partCar);
                    }

                    if (carPartsToAdd.Count > 0)
                    {
                        currentCar.PartCars = carPartsToAdd;
                        context.PartCars.AddRange(carPartsToAdd);
                        carPartsToAdd.Clear();
                    }
                }
            }

            context.SaveChanges();

            return $"Successfully imported {context.Cars.ToList().Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context
                .Customers
                .OrderBy(c => c.BirthDate).ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    c.IsYoungDriver
                })
                .ToList();

            var json = JsonConvert.SerializeObject(customers, settings);

            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                })
                .OrderBy(c => c.Model).ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            var json = JsonConvert.SerializeObject(cars, settings);

            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(suppliers, settings);

            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    parts = c.PartCars
                                .Select(pc => new
                                {
                                    pc.Part.Name,
                                    Price = $"{pc.Part.Price:f2}"
                                }).ToList()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(cars, settings);

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney).ThenByDescending(c => c.BoughtCars)
                .ToList();

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            settings.ContractResolver = resolver;

            var json = JsonConvert.SerializeObject(customers, settings);

            return json;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = s.Discount.ToString("f2"),
                    price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("f2"),
                    priceWithDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price) - (s.Car.PartCars.Sum(pc => pc.Part.Price) * (s.Discount / 100))).ToString("f2")
                })
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(sales, settings);

            return json;
        }
    }
}