namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        // JSON
        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesImport = JsonConvert.DeserializeObject<List<Movie>>(jsonString);

            var movies = new List<Movie>();
            var sb = new StringBuilder();

            foreach (var currentMovie in moviesImport)
            {
                var movieExists = movies.Any(t => t.Title == currentMovie.Title);
                var isValidMovie = IsValid(currentMovie); // validate all properties attributes
                var isValidEnum = Enum.TryParse(typeof(Genre), currentMovie.Genre.ToString(), out object genre); // validate the enum separately

                if (movieExists || !isValidMovie || !isValidEnum)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = new Movie
                {
                    Id = currentMovie.Id,
                    Title = currentMovie.Title,
                    Genre = currentMovie.Genre,
                    Duration = currentMovie.Duration,
                    Rating = currentMovie.Rating,
                    Director = currentMovie.Director,
                    Projections = currentMovie.Projections
                };

                movies.Add(movie);

                sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, movie.Rating.ToString("F2")));
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        // JSON
        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsImportDTOs = JsonConvert.DeserializeObject<List<HallImportDTO>>(jsonString);
            
            var halls = new List<Hall>();
            var sb = new StringBuilder();

            foreach (var currentHallDTO in hallsImportDTOs)
            {
                var isValidHall = IsValid(currentHallDTO);
                
                if (!isValidHall)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall
                {
                      Name = currentHallDTO.Name,
                      Is4Dx = currentHallDTO.Is4Dx,
                      Is3D = currentHallDTO.Is3D
                };

                for (int i = 0; i < currentHallDTO.Seats; i++)
                {
                    hall.Seats.Add(new Seat());
                }

                halls.Add(hall);

                string status = "";

                if (hall.Is4Dx)
                {
                    status = hall.Is3D ? "4Dx/3D" : "4Dx";
                }
                else if (hall.Is3D)
                {
                    status = "3D";
                }
                else
                {
                    status = "Normal";
                }

                sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, status, hall.Seats.Count));
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        // XML
        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));

            var projectionsDto = (ImportProjectionDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            var projections = new List<Projection>();
            var sb = new StringBuilder();

            foreach (var projectionDto in projectionsDto)
            {
                var movie = context.Movies.Find(projectionDto.MovieId);
                var hall = context.Halls.Find(projectionDto.HallId);

                if (movie == null || hall == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = new Projection
                {
                    MovieId = projectionDto.MovieId,
                    HallId = projectionDto.HallId,
                    DateTime = DateTime.ParseExact(projectionDto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };

                projections.Add(projection);

                sb.AppendLine(string.Format(SuccessfulImportProjection, movie.Title, projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();

            return sb.ToString();
        }

        // XML
        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportTicketDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (ImportTicketDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            var customers = new List<Customer>();
            var sb = new StringBuilder();

            foreach (var customerDto in customersDto)
            {
                var projections = context.Projections.Select(x => x.Id).ToArray();
                var projectionExists = projections.Any(x => customerDto.Tickets.Any(s => s.ProjectionId != x));

                if (!IsValid(customerDto) && projectionExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Age = customerDto.Age,
                    Balance = customerDto.Balance,
                };

                foreach (var ticket in customerDto.Tickets)
                {
                    customer.Tickets.Add(new Ticket
                    {
                        ProjectionId = ticket.ProjectionId,
                        Price = ticket.Price
                    });
                }

                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName, customer.Tickets.Count));

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}