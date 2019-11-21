namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        // JSON
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context
                .Movies
                .Where(m => m.Rating >= rating && m.Projections.Any(p => p.Tickets.Count >= 1))
                .OrderByDescending(r => r.Rating)
                .ThenByDescending(p => p.Projections.Sum(t => t.Tickets.Sum(pc => pc.Price)))
                .Select(m => new
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("f2"),
                    TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("f2"),
                    Customers = m.Projections
                                    .SelectMany(p => p.Tickets)
                                    .Select(t => new
                                    {
                                        FirstName = t.Customer.FirstName,
                                        LastName = t.Customer.LastName,
                                        Balance = t.Customer.Balance.ToString("F2"),
                                    })
                                    .OrderByDescending(c => c.Balance)
                                    .ThenBy(c => c.FirstName)
                                    .ThenBy(c => c.LastName)
                                    .ToList()
                })
                .Take(10)
                .ToList();

            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            var json = JsonConvert.SerializeObject(movies, settings);

            return json;
        }

        // XML
        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<CustomerDto>), new XmlRootAttribute("Customers"));

            var stringBuilder = new StringBuilder();

            var customoers = context
                .Customers
                .Where(c => c.Age >= age)
                .OrderByDescending(c => c.Tickets.Sum(t => t.Price))
                .Take(10)
                .Select(c => new CustomerDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = c.Tickets.Sum(t => t.Price).ToString("f2"),
                    SpentTime = TimeSpan.FromSeconds(c.Tickets.Sum(s => s.Projection.Movie.Duration.TotalSeconds)).ToString(@"hh\:mm\:ss")
                })
                .ToList();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), customoers, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}