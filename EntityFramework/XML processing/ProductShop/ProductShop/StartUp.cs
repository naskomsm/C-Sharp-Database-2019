namespace ProductShop
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
    using ProductShop.Data;
    using ProductShop.DTOs;
    using ProductShop.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var dbContext = new ProductShopContext();

            var xmlUsers = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Product Shop\ProductShop\Datasets\users.xml");
            var xmlProducts = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Product Shop\ProductShop\Datasets\products.xml");
            var xmlCategories = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Product Shop\ProductShop\Datasets\categories.xml");
            var xmlCategoriesProducts = File.ReadAllText(@"C:\Users\pc1\Downloads\01. Import Users_Product Shop\ProductShop\Datasets\categories-products.xml");

            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            dbContext.Database.Migrate();

            using (dbContext)
            {
                //Seed->import
                //Console.WriteLine(ImportUsers(dbContext, xmlUsers));
                //Console.WriteLine(ImportProducts(dbContext, xmlProducts));
                //Console.WriteLine(ImportCategories(dbContext, xmlCategories));
                //Console.WriteLine(ImportCategoryProducts(dbContext, xmlCategoriesProducts));

                // Export
                Console.WriteLine(GetUsersWithProducts(dbContext));
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<UserImputDTO>), new XmlRootAttribute("Users"));
            var usersDTOs = (List<UserImputDTO>)serializer.Deserialize(new StringReader(inputXml));

            var users = Mapper.Map<List<UserImputDTO>, List<User>>(usersDTOs);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ProductImputDTO>), new XmlRootAttribute("Products"));
            var productsDTOs = (List<ProductImputDTO>)serializer.Deserialize(new StringReader(inputXml));

            var products = Mapper.Map<List<ProductImputDTO>, List<Product>>(productsDTOs);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}"; ;
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoryImputDTO>), new XmlRootAttribute("Categories"));
            var categoriesDTOs = (List<CategoryImputDTO>)serializer.Deserialize(new StringReader(inputXml));

            var categories = Mapper.Map<List<CategoryImputDTO>, List<Category>>(categoriesDTOs);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoryProductImputDTO>), new XmlRootAttribute("CategoryProducts"));
            var categoriyProductsDTOs = (List<CategoryProductImputDTO>)serializer.Deserialize(new StringReader(inputXml));

            var categoryProducts = Mapper.Map<List<CategoryProductImputDTO>, List<CategoryProduct>>(categoriyProductsDTOs);

            var categoryIds = context.Categories.Select(c => c.Id);
            var productsIds = context.Products.Select(p => p.Id);

            var validCategoryProducts = categoryProducts.Where(cp => categoryIds.Contains(cp.CategoryId)
                                                                    && productsIds.Contains(cp.ProductId))
                                                                    .ToList();

            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ProductsInRangeExportDTO>() // SELECT
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<ProductsInRangeExportDTO>), new XmlRootAttribute("Products"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), products, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<GetSoldProductsExportDTO>()
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<GetSoldProductsExportDTO>), new XmlRootAttribute("Users"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), users, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .ProjectTo<CategoriesByProductsExportDTO>()
                .OrderByDescending(x => x.NumberOfProducts)
                .ThenBy(x => x.TotalPriceSum)
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<CategoriesByProductsExportDTO>), new XmlRootAttribute("Categories"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), categories, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderByDescending(u => u.ProductsSold.Count)
                .ProjectTo<UsersAndProductsExportDTO>()
                .ToList();

            foreach (var user in users)
            {
                user.ProductsSold.Products = user.ProductsSold.Products.OrderBy(x => x.Price).ToArray();
            }

            var xmlSerializer = new XmlSerializer(typeof(UsersAndProductsExportDTO[]), new XmlRootAttribute("Users"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), users, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}