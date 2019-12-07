namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ImportDto.JSON;
    using System.Text;
    using TeisterMask.Data.Models;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ImportDto.XML;
    using System.IO;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ImportProjectDto>), new XmlRootAttribute("Projects"));
            var projectsDTOs = (List<ImportProjectDto>)serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var projects = new List<Project>();
            var tasks = new List<Task>();

            foreach (var dto in projectsDTOs)
            {
                var isProjectValid = IsValid(dto);

                if (!isProjectValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var project = new Project()
                {
                    Name = dto.Name,
                    OpenDate = DateTime.ParseExact(dto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                };

                foreach (var taskDto in dto.Tasks)
                {
                    var taskOpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var taskDueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var isTaskValid = IsValid(taskDto);
                    var isOpenDateValid = taskDto.OpenDate != null ? true : false;
                    var isDueDateValid = taskDto.DueDate != null ? true : false;

                    var isTaskOpenDateBeforeProjectOpenDate = taskOpenDate < project.OpenDate;
                    var isTaskDueDateAfterProjectDueDate = taskDueDate > project.DueDate;
                    
                    var areEqual1 = taskOpenDate == project.OpenDate;
                    var areEqual2 = taskDueDate == project.DueDate;

                    if (areEqual1 || areEqual2)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(project.DueDate == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(!isTaskValid || !isOpenDateValid || !isDueDateValid 
                        || isTaskOpenDateBeforeProjectOpenDate || isTaskDueDateAfterProjectDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var task = new Task()
                    {
                        Name = taskDto.Name,
                        DueDate = taskDueDate,
                        OpenDate = taskOpenDate,
                        ExecutionType = (ExecutionType)Enum.Parse(typeof(ExecutionType), taskDto.ExecutionType, true),
                        LabelType = (LabelType)Enum.Parse(typeof(LabelType), taskDto.LabelType, true)
                    };

                    tasks.Add(task);
                    project.Tasks.Add(task);
                }

                projects.Add(project);
                var result = string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count);
                sb.AppendLine(result);
            }

            context.Tasks.AddRange(tasks);
            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesImport = JsonConvert.DeserializeObject<List<ImportEmployeesDto>>(jsonString);

            var allTasks = context.Tasks.Select(t => t.Id).ToList();
           
            var employeeTasks = new List<EmployeeTask>();
            var employees = new List<Employee>();
            var sb = new StringBuilder();

            foreach (var dto in employeesImport)
            {
                var isEmployeeValid = IsValid(dto);

                if (!isEmployeeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = new Employee()
                {
                    Username = dto.Username,
                    Phone = dto.Phone,
                    Email = dto.Email,
                };

                var tasksToLoop = dto.Tasks.Distinct();

                foreach (var taskDto in tasksToLoop)
                {
                    if (!allTasks.Contains(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var employeeTask = new EmployeeTask()
                    {
                        Employee = employee,
                        TaskId = taskDto
                    };

                    employeeTasks.Add(employeeTask);
                    employee.EmployeesTasks.Add(employeeTask);
                }

                employees.Add(employee);
                var result = string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count);
                sb.AppendLine(result);
            }

            context.Employees.AddRange(employees);
            context.EmployeesTasks.AddRange(employeeTasks);
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