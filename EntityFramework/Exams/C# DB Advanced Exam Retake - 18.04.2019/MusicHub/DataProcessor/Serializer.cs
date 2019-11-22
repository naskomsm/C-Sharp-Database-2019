namespace MusicHub.DataProcessor
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
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context
                .Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        Price = s.Price.ToString("f2"),
                        Writer = s.Writer.Name
                    })
                                                .OrderByDescending(s => s.SongName)
                                                .ThenBy(s => s.Writer)
                                                .ToList(),
                    AlbumPrice = a.Price.ToString("f2")
                })
                .OrderByDescending(a => decimal.Parse(a.AlbumPrice))
                .ToList();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(albums, settings);

            return json;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<SongExportDTO>), new XmlRootAttribute("Songs"));
            var stringBuilder = new StringBuilder();

            var songs = context
                .Songs
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new SongExportDTO
                {
                    Name = s.Name,
                    WriterName = s.Writer.Name,
                    PerformerName = s.SongPerformers.Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName).FirstOrDefault(),
                    AlbumProducerName = s.Album.Producer.Name,
                    Duration = s.Duration.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture)
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ThenBy(s => s.PerformerName)
                .ToList();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), songs, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}