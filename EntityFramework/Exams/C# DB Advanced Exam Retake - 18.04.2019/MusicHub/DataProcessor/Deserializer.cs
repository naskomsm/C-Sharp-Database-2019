namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
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

        // JSON
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

        // JSON
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
                    if (newProducer.PhoneNumber == null)
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

        // XML
        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<SongImportDTO>), new XmlRootAttribute("Songs"));
            var songsDTOs = (List<SongImportDTO>)serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var songs = new List<Song>();

            var albumsIds = context.Albums.Select(a => a.Id).ToList();
            var writersIds = context.Writers.Select(w => w.Id).ToList();

            foreach (var dto in songsDTOs)
            {
                var isValid = IsValid(dto);
                var doesSongExist = songs.Any(s => s.Name == dto.Name);

                if (!isValid
                    || dto.AlbumId == null
                    || !albumsIds.Contains((int)dto.AlbumId)
                    || !writersIds.Contains(dto.WriterId)
                    || Enum.TryParse(dto.Genre, out Genre myStatus) == false
                    || doesSongExist
                 )
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var song = AutoMapper.Mapper.Map<Song>(dto);

                var result = String.Format(SuccessfullyImportedSong, song.Name, song.Genre, song.Duration);
                sb.AppendLine(result);
                songs.Add(song);
            }

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<PerformerImputDTO>), new XmlRootAttribute("Performers"));
            var performersDTOs = (List<PerformerImputDTO>)serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var performers = new List<Performer>();

            var validSongs = context.Songs.Select(s => s.Id).ToList();

            foreach (var dto in performersDTOs)
            {
                var isValid = IsValid(dto);

                if (!isValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var performer = new Performer()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    NetWorth = dto.NetWorth
                };

                var isOkToImport = true;
                foreach (var song in dto.PerformerSongs)
                {
                    if (!validSongs.Contains(song.Id))
                    {
                        sb.AppendLine(ErrorMessage);
                        isOkToImport = false;
                        break;
                    }

                    var songPerformer = new SongPerformer()
                    {
                        SongId = song.Id
                    };


                    performer.PerformerSongs.Add(songPerformer);
                }

                if (isOkToImport)
                {
                    var result = String.Format(SuccessfullyImportedPerformer, performer.FirstName, performer.PerformerSongs.Count());
                    sb.AppendLine(result);
                    performers.Add(performer);
                }
            }

            context.Performers.AddRange(performers);
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