namespace MusicHub.DataProcessor
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
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writers = JsonConvert.DeserializeObject<List<WriterImportDTO>>(jsonString);

            var writersToAdd = new HashSet<Writer>();
            var sb = new StringBuilder();

            foreach (var writer in writers)
            {
                if (!IsValid(writer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newWriter = new Writer
                {
                    Name = writer.Name,
                    Pseudonym = writer.Pseudonym
                };

                writersToAdd.Add(newWriter);

                sb.AppendLine(string.Format(SuccessfullyImportedWriter, newWriter.Name));
            }

            context.Writers.AddRange(writersToAdd);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producersDTOs = JsonConvert.DeserializeObject<List<ProducerImportDTO>>(jsonString);

            var producersToAdd = new HashSet<Producer>();
            var albumsToAdd = new HashSet<Album>();
            var sb = new StringBuilder();

            foreach (var producerDTO in producersDTOs)
            {
                if (!IsValid(producerDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newProducer = new Producer()
                {
                    Name = producerDTO.Name,
                    Pseudonym = producerDTO.Pseudonym,
                    PhoneNumber = producerDTO.PhoneNumber
                };

                var areAlbumsOk = true;
                foreach (var albumDTO in producerDTO.Albums)
                {
                    if (!IsValid(albumDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        areAlbumsOk = false;
                        break;
                    }

                    var newAlbum = new Album()
                    {
                        Name = albumDTO.Name,
                        ReleaseDate = DateTime.ParseExact(albumDTO.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Producer = newProducer,
                        ProducerId = newProducer.Id
                    };

                    albumsToAdd.Add(newAlbum);
                }

                if (areAlbumsOk) 
                {
                    newProducer.Albums = albumsToAdd;
                    producersToAdd.Add(newProducer);

                    var result = ""; 
                    if(newProducer.PhoneNumber == null)
                    {
                        result = string.Format(SuccessfullyImportedProducerWithNoPhone, newProducer.Name, newProducer.Albums.Count);
                    }

                    else
                    {
                        result = string.Format(SuccessfullyImportedProducerWithPhone, newProducer.Name, newProducer.PhoneNumber, newProducer.Albums.Count);
                    }
                    
                    sb.AppendLine(result);
                }

                albumsToAdd = new HashSet<Album>();
            }

            context.Producers.AddRange(producersToAdd);
            context.Albums.AddRange(albumsToAdd);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportSongDto[]), new XmlRootAttribute("Songs"));
            var songDtos = (ImportSongDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var validSongs = new List<Song>();

            foreach (var songDto in songDtos)
            {
                if (!IsValid(songDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var genre = Enum.TryParse(songDto.Genre, out Genre genreResult);
                var album = context.Albums.Find(songDto.AlbumId);
                var writer = context.Writers.Find(songDto.WriterId);
                var songTitle = validSongs.Any(s => s.Name == songDto.Name);

                if (!genre || album == null || writer == null || songTitle)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var song = AutoMapper.Mapper.Map<Song>(songDto);

                sb.AppendLine(string.Format(SuccessfullyImportedSong, song.Name, song.Genre, song.Duration));
                validSongs.Add(song);
            }

            context.Songs.AddRange(validSongs);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();

            return result;
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var performerDtos = DeserializeObject<ImportPerformerDto>("Performers", xmlString);

            var validPerformers = new List<Performer>();
            var sb = new StringBuilder();

            foreach (var performerDto in performerDtos)
            {
                if (!IsValid(performerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validSongsCount = context.Songs.Count(s => performerDto.PerformerSongs.Any(i => i.Id == s.Id));

                if (validSongsCount != performerDto.PerformerSongs.Length)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var performer = AutoMapper.Mapper.Map<Performer>(performerDto);

                validPerformers.Add(performer);
                sb.AppendLine(string.Format(SuccessfullyImportedPerformer, performer.FirstName,
                    performer.PerformerSongs.Count));
            }

            context.Performers.AddRange(validPerformers);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();

            return result;
        }

        // ...
        private static T[] DeserializeObject<T>(string rootElement, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(rootElement));
            var deserializedDtos = (T[])xmlSerializer.Deserialize(new StringReader(xmlString));
            return deserializedDtos;
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}