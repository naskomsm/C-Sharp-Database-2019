namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Globalization;
    using SoftJail.DataProcessor.ImportDto.JSON;
    using System.Xml.Serialization;
    using SoftJail.DataProcessor.ImportDto.XML;
    using System.IO;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsCells = JsonConvert.DeserializeObject<List<Department>>(jsonString);

            var departments = new List<Department>();
            var cells = new List<Cell>();

            var sb = new StringBuilder();

            foreach (var departmentCell in departmentsCells)
            {
                var isValid = IsValid(departmentCell);

                if (!isValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var department = new Department()
                {
                    Name = departmentCell.Name
                };

                var shouldContinue = true;
                foreach (var currentCell in departmentCell.Cells)
                {
                    var isCellValid = IsValid(currentCell);

                    if (!isCellValid)
                    {
                        sb.AppendLine("Invalid Data");
                        shouldContinue = false;
                        break;
                    }

                    var cell = new Cell()
                    {
                        CellNumber = currentCell.CellNumber,
                        HasWindow = currentCell.HasWindow
                    };

                    department.Cells.Add(cell);
                    cells.Add(cell);
                }

                if (shouldContinue)
                {
                    departments.Add(department);
                    sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                }
            }

            context.Departments.AddRange(departments);
            context.Cells.AddRange(cells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersMails = JsonConvert.DeserializeObject<List<PrisonersMailsDTO>>(jsonString);

            var prisoners = new List<Prisoner>();
            var mails = new List<Mail>();

            var sb = new StringBuilder();

            foreach (var prisonerMail in prisonersMails)
            {
                var isPrisonerValid = IsValid(prisonerMail);

                if (!isPrisonerValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var releaseDate = prisonerMail.ReleaseDate == null ? (DateTime?)null
                    : DateTime.ParseExact(prisonerMail.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var prisoner = new Prisoner()
                {
                    FullName = prisonerMail.FullName,
                    Nickname = prisonerMail.Nickname,
                    Age = prisonerMail.Age,
                    IncarcerationDate = DateTime.ParseExact(prisonerMail.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = releaseDate,
                    Bail = prisonerMail.Bail,
                    CellId = prisonerMail.CellId,
                };

                var shouldContinue = true;
                foreach (var currentMail in prisonerMail.Mails)
                {
                    var isMailValid = IsValid(currentMail.Address);

                    if (!isMailValid)
                    {
                        shouldContinue = false;
                        sb.AppendLine("Invalid Data");
                        break;
                    }

                    var mail = new Mail()
                    {
                        Description = currentMail.Description,
                        Sender = currentMail.Sender,
                        Address = currentMail.Address
                    };

                    prisoner.Mails.Add(mail);
                    mails.Add(mail);
                }

                if (shouldContinue)
                {
                    prisoners.Add(prisoner);
                    sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
                }
            }

            context.Prisoners.AddRange(prisoners);
            context.Mails.AddRange(mails);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<OfficersPrisonersDTO>), new XmlRootAttribute("Officers"));
            var officersPrisonersDTOs = (List<OfficersPrisonersDTO>)serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var officersPrisoners = new List<OfficerPrisoner>();
            var officers = new List<Officer>();

            foreach (var officerPrisonerDTO in officersPrisonersDTOs)
            {
                var isOfficerValid = IsValid(officerPrisonerDTO);
                var position = Enum.TryParse(officerPrisonerDTO.Position, out Position myStatus);
                var weapon = Enum.TryParse(officerPrisonerDTO.Weapon, out Weapon myStatus2);

                if (!isOfficerValid || position == false || weapon == false)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer()
                {
                    FullName = officerPrisonerDTO.FullName,
                    Salary = officerPrisonerDTO.Salary,
                    Position = (Position)Enum.Parse(typeof(Position), officerPrisonerDTO.Position, true),
                    Weapon = (Weapon)Enum.Parse(typeof(Weapon), officerPrisonerDTO.Weapon, true),
                    DepartmentId = officerPrisonerDTO.DepartmentId
                };

                foreach (var prisonerDTO in officerPrisonerDTO.Prisoners)
                {
                    var officerPrisoner = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = prisonerDTO.Id
                    };

                    officer.OfficerPrisoners.Add(officerPrisoner);
                    officersPrisoners.Add(officerPrisoner);
                }

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
                officers.Add(officer);
            }

            context.Officers.AddRange(officers);
            context.OfficersPrisoners.AddRange(officersPrisoners);
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