namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.enums;
    using VaporStore.DataProcessor.XMLDtos.export;
    using Formatting = Newtonsoft.Json.Formatting;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context
                .Genres
                .Where(g => g.Games.Any(ga => ga.Purchases.Count() >= 1) && genreNames.Contains(g.Name))
                .OrderByDescending(g => g.Games.Sum(ga => ga.Purchases.Count))
                .ThenBy(g => g.Id)
                .Select(g => new
                {
                    g.Id,
                    Genre = g.Name,
                    Games = g.Games.Select(ga => new
                    {
                        Id = ga.Id,
                        Title = ga.Name,
                        Developer = ga.Developer.Name,
                        Tags = string.Join(", ", ga.GameTags.Select(gt => gt.Tag.Name).ToList()),
                        Players = ga.Purchases.Count()
                    })
                                                .Where(ga => ga.Players >= 1)
                                                .OrderByDescending(ga => ga.Players)
                                                .ThenBy(ga => ga.Id),
                    TotalPlayers = g.Games.Sum(ga => ga.Purchases.Count)
                })
                .ToList();

            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            var json = JsonConvert.SerializeObject(genres, settings);

            return json;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<UserExportDTO>), new XmlRootAttribute("Users"));
            var stringBuilder = new StringBuilder();

            var storeTypeValue = Enum.Parse<PurchaseType>(storeType);

            var users = context
                .Users
                .Select(u => new UserExportDTO
                {
                    Username = u.Username,
                    Purchases = u.Cards
                            .SelectMany(c => c.Purchases)
                            .Where(p => p.Type == storeTypeValue)
                            .Select(p => new PurchaseExportDTO
                            {
                                Card = p.Card.Number,
                                Cvc = p.Card.Cvc,
                                Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                                Game = new GameExportDTO
                                {
                                    Name = p.Game.Name,
                                    Genre = p.Game.Genre.Name,
                                    Price = p.Game.Price,
                                }
                            })
                            .OrderBy(x => x.Date)
                            .ToArray(),
                    TotalSpent = u.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == storeTypeValue)
                        .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Any())
				.OrderByDescending(u => u.TotalSpent)
				.ThenBy(u => u.Username)
				.ToList();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), users, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}