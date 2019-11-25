namespace VaporStore.DataProcessor
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
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.JSONDtos.import;
    using VaporStore.DataProcessor.XMLDtos.import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDTO = JsonConvert.DeserializeObject<List<GamesDTO>>(jsonString);

            var genresStrings = new List<string>();
            var developersStrings = new List<string>();
            var tagsStrings = new List<string>();

            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();

            var games = new List<Game>();

            var sb = new StringBuilder();

            foreach (var dto in gamesDTO)
            {
                var isValid = IsValid(dto);

                if (!isValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (dto.Name == null || dto.ReleaseDate == null
                    || dto.Developer == null || dto.Genre == null || dto.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var genre = new Genre() { Name = dto.Genre };
                if (!genresStrings.Contains(genre.Name))
                {
                    genresStrings.Add(genre.Name);
                    genres.Add(genre);
                }

                var developer = new Developer() { Name = dto.Developer };
                if (!developersStrings.Contains(developer.Name))
                {
                    developersStrings.Add(developer.Name);
                    developers.Add(developer);
                }

                var game = new Game()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    ReleaseDate = DateTime.ParseExact(dto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = developer,
                    Genre = genre
                };

                foreach (var tagName in dto.Tags)
                {
                    var tag = new Tag()
                    {
                        Name = tagName
                    };

                    var gameTag = new GameTag()
                    {
                        Tag = tag
                    };

                    if (!tagsStrings.Contains(tag.Name))
                    {
                        tags.Add(tag);
                        tagsStrings.Add(tag.Name);
                    }

                    game.GameTags.Add(gameTag);
                }

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
                games.Add(game);
            }

            context.Genres.AddRange(genres);
            context.Games.AddRange(games);
            context.Tags.AddRange(tags);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDTO = JsonConvert.DeserializeObject<List<UserDTO>>(jsonString);

            var cards = new List<Card>();
            var users = new List<User>();

            var sb = new StringBuilder();

            foreach (var dto in usersDTO)
            {
                var isValid = IsValid(dto);

                if (!isValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (dto.Cards.Count == 0 || dto.FullName == "" || dto.Email == "")
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User()
                {
                    FullName = dto.FullName,
                    Username = dto.Username,
                    Email = dto.Email,
                    Age = dto.Age
                };

                foreach (var card in dto.Cards)
                {
                    var isCardValid = IsValid(card);

                    if (!isCardValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    var newCard = new Card()
                    {
                        Number = card.Number,
                        Cvc = card.CVC,
                        Type = card.Type
                    };

                    user.Cards.Add(newCard);
                    cards.Add(newCard);
                }

                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.Cards.AddRange(cards);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<PurchaseDTO>), new XmlRootAttribute("Purchases"));
            var purchasesDTOs = (List<PurchaseDTO>)serializer.Deserialize(new StringReader(xmlString));

            var purchases = new List<Purchase>();
            var sb = new StringBuilder();

            foreach (var dto in purchasesDTOs)
            {
                var isValid = IsValid(dto);

                if (!isValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase()
                {
                    Type = dto.Type,
                    ProductKey = dto.Key,
                    Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                };

                var user = context.Users.FirstOrDefault(x => x.Cards.Any(c => c.Number == dto.Number));
                sb.AppendLine($"Imported {dto.Title} for {user.Username}");
                purchases.Add(purchase);
            }

            context.Purchases.AddRange(purchases);
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