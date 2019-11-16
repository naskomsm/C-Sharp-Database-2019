namespace ProductShop
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using ProductShop.Data;
    using ProductShop.Models;

    public class StartUp
    {
        static DefaultContractResolver resolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = resolver
        };

        public static void Main()
        {
            var dbContext = new ProductShopContext();

            var jsonUsers = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Product Shop\ProductShop\Datasets\users.json");
            var jsonProducts = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Product Shop\ProductShop\Datasets\products.json");
            var jsonCategories = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Product Shop\ProductShop\Datasets\categories.json");
            var jsonCategoriesProducts = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Product Shop\ProductShop\Datasets\categories-products.json");

            dbContext.Database.Migrate();

            using (dbContext)
            {
                // Seed
                Console.WriteLine(ImportUsers(dbContext, jsonUsers));
                Console.WriteLine(ImportProducts(dbContext, jsonProducts));
                Console.WriteLine(ImportCategories(dbContext, jsonCategories));
                Console.WriteLine(ImportCategoryProducts(dbContext, jsonCategoriesProducts));

                // Queries
                Console.WriteLine(GetCategoriesByProductsCount(dbContext));
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            var validCategories = categories
                .Where(c => c.Name != null)
                .ToList();

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .OrderBy(p => p.Price)
                .ToList();

            var json = JsonConvert.SerializeObject(products, settings);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1 && u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                                        .Select(pd => new
                                        {
                                            pd.Name,
                                            pd.Price,
                                            BuyerFirstName = pd.Buyer.FirstName,
                                            BuyerLastName = pd.Buyer.LastName
                                        })
                                        .ToList()
                })
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .ToList();

            var json = JsonConvert.SerializeObject(users, settings);

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new 
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = $"{c.CategoryProducts.Average(cp => cp.Product.Price):f2}",
                    TotalRevenue = $"{c.CategoryProducts.Sum(cp => cp.Product.Price):f2}"
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToList();

            var json = JsonConvert.SerializeObject(categories, settings);

            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1 && u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.Buyer != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,

                    SoldProducts = new
                    {
                        Count = u.ProductsSold.Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                                    .Where(p => p.Buyer != null)
                                    .Select(p => new
                                    {
                                        p.Name,
                                        p.Price
                                    })
                                    .ToList()
                    }
                })
                .ToList();

            settings.NullValueHandling = NullValueHandling.Ignore;

            var result = new
            {
                usersCount = users.Count,
                users
            };

            var json = JsonConvert.SerializeObject(result, settings);

            return json;
        }
    }
}