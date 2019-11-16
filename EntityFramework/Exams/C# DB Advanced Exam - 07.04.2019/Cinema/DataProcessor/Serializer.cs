namespace Cinema.DataProcessor
{
    using System;
    using System.Linq;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

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
            throw new NotImplementedException();
        }
    }
}