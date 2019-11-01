namespace SoftUni
{
    using SoftUni.Data;
    using SoftUni.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            var context = new SoftUniContext();

            using (context)
            {
                var result = StartUp.GetEmployeesFullInformation(context);
                Console.WriteLine(result);
            }
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return result.ToString();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return result.ToString();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department,
                    e.Salary
                })
                .OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName)
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:F2}");
            }

            return result.ToString();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAdress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAdress);

            var employeeToSetAdress = context.Employees
                .Where(e => e.LastName == "Nakov")
                .FirstOrDefault();

            employeeToSetAdress.Address = newAdress;

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => new
                {
                    AdressText = e.Address.AddressText
                })
                .Take(10)
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.AdressText}");
            }

            context.SaveChanges();

            return result.ToString();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerName = e.Manager.FirstName + " " + e.Manager.LastName,
                    Projects = e.EmployeesProjects
                                                .Select(p => new
                                                {
                                                    p.Project.Name,
                                                    p.Project.StartDate,
                                                    p.Project.EndDate
                                                }).ToList()
                })
                .Take(10)
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerName}");

                string format = "M/d/yyyy h:mm:ss tt";
                
                foreach (var project in employee.Projects)
                {
                    string startDate = project.StartDate.ToString(format, CultureInfo.InvariantCulture);

                    string endDate = project.EndDate != null 
                        ? project.EndDate.Value.ToString(format, CultureInfo.InvariantCulture)
                        : "not finished";

                    result.AppendLine($"--{project.Name} - {startDate} - {endDate}");
                }
            }

            return result.ToString();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var adresses = context.Addresses
                .Select(a => new
                {
                    AdressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeesCount).ThenBy(a => a.TownName).ThenBy(a => a.AdressText)
                .Take(10)
                .ToList();

            var result = new StringBuilder();

            foreach (var adress in adresses)
            {
                result.AppendLine($"{adress.AdressText}, {adress.TownName} - {adress.EmployeesCount} employees");
            }

            return result.ToString();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => new
                    {
                        p.Project.Name
                    }).OrderBy(p => p.Name)
                })
                .FirstOrDefault();

            var result = new StringBuilder();

            result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var project in employee.Projects)
            {
                result.AppendLine($"{project.Name}");
            }

            return result.ToString();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count).ThenBy(x => x.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerName = d.Manager.FirstName + " " + d.Manager.LastName,
                    Employees = d.Employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                })
                .ToList();


            var result = new StringBuilder();

            foreach (var department in departments)
            {
                result.AppendLine($"{department.Name} - {department.ManagerName}");

                foreach (var employee in department.Employees)
                {
                    result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return result.ToString();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .ToList();


            var result = new StringBuilder();

            foreach (var project in projects)
            {
                string format = "M/d/yyyy h:mm:ss tt";
                string startDate = project.StartDate.ToString(format, CultureInfo.InvariantCulture);

                result.AppendLine($"{project.Name}");
                result.AppendLine($"{project.Description}");
                result.AppendLine($"{project.StartDate}");
            }

            return result.ToString();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var departmentsAllowed = new List<string>()
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            var employeesToModify = context.Employees
                .Where(e => departmentsAllowed.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
                .ToList();

            foreach (var employee in employeesToModify)
            {
                employee.Salary += employee.Salary * (decimal)0.12;
            }

            context.SaveChanges();

            var result = new StringBuilder();

            var engineers = context.Employees
                .Where(employee => departmentsAllowed.Contains(employee.Department.Name))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var e in engineers)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                     e.FirstName,
                     e.LastName,
                     e.JobTitle,
                     e.Salary
                })
                .OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return result.ToString();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeesProjects = context.EmployeesProjects
                .ToList();

            var itemToRemove = context.EmployeesProjects
                .Where(x => x.EmployeeId == 2)
                .FirstOrDefault();

            employeesProjects.Remove(itemToRemove);
            
            context.SaveChanges();

            var employees = context.Employees
                .ToList();

            var secondItemToRemove = context.Employees.Find(2);


            employees.Remove(secondItemToRemove);

            context.SaveChanges();

            var projects = context.Projects
                .Select(e => e.Name)
                .Take(10)
                .ToList();

            var result = new StringBuilder();

            foreach (var project in projects)
            {
                result.AppendLine($"{project}");
            }

            return result.ToString();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var addressesToDelete = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .Select(a => a.AddressId)
                .ToList();

            var allAdresses = context.Addresses
                .ToList();

            var employees = context.Employees
                .Where(e => addressesToDelete.Contains(e.Address.AddressId))
                .ToList();

            foreach (var employee in employees)
            {
                employee.AddressId = null;
            }

            foreach (var address in allAdresses)
            {
                if (addressesToDelete.Contains(address.AddressId))
                {
                    context.Addresses.Remove(address);
                }
            }

            var townsToDelete = context.Towns
                 .Where(t => t.Name == "Seattle")
                 .ToList();

            foreach (var townToDelete in townsToDelete)
            {
                context.Towns.Remove(townToDelete);
            }

            context.SaveChanges();

            return $"{addressesToDelete.Count} addresses in Seattle were deleted";
        }
    }
}
