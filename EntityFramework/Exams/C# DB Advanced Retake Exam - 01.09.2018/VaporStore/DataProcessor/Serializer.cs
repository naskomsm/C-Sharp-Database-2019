namespace VaporStore.DataProcessor
{
    using System;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;

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
            //todo
            return "";
        }
    }
}