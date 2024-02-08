using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            // 3. Employees Full Information
            //Console.WriteLine(GetEmployeesFullInformation(context));

            // 4. Employees with Salary Over 50 000
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));

            // 5. Employees from Research and Development
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));

            // 6. Adding a New Address and Updating Employee
            //Console.WriteLine(AddNewAddressToEmployee(context));

            // 7. Employees and Projects
            //Console.WriteLine(GetEmployeesInPeriod(context));

            // 8. Addresses by Town
            //Console.WriteLine(GetAddressesByTown(context));

            // 9. Employee 147
            //Console.WriteLine(GetEmployee147(context));

            // 10. Departments with More Than 5 Employees
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));

            // 11. Find Latest 10 Projects
            //Console.WriteLine(GetLatestProjects(context));

            // 12. Increase Salaries
            //Console.WriteLine(IncreaseSalaries(context));

            // 13. Find Employees by First Name Starting with "Sa"
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));

            // 14. Delete Project by Id
            //Console.WriteLine(DeleteProjectById(context));

            // 15. Remove Town
            //Console.WriteLine(RemoveTown(context));
        }

        // 3. Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employeesInfo = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new { e.FirstName, e.LastName, e.MiddleName, e.JobTitle, e.Salary })
                .ToList();

            string result = string.Join(Environment.NewLine, employeesInfo.Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}"));

            return result;
        }

        // 4. Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employeesWithSalaryOver50000 = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new { e.FirstName, e.Salary })
                .OrderBy(e => e.FirstName)
                .ToList();

            return string.Join(Environment.NewLine, employeesWithSalaryOver50000.Select(e => $"{e.FirstName} - {e.Salary:f2}"));
        }

        // 5. Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new { e.FirstName, e.LastName, e.Department, e.Salary })
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName)
                .ToList();

            string result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}"));

            return result;
        }

        // 6. Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            employee.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .Select(e => new { e.AddressId, e.Address.AddressText })
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .ToList();

            string result = string.Join(Environment.NewLine, employees.Select(e => $"{e.AddressText}"));

            return result;
        }

        // 7. Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employeeInfo = context.Employees
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001 & ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = ep.Project.EndDate != null
                                ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                : "not finished"
                        })
                        .ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employeeInfo)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                if (e.Projects.Any())
                {
                    sb.AppendLine(string.Join(Environment.NewLine, e.Projects
                        .Select(p => $"--{p.ProjectName} - {p.StartDate} - {p.EndDate}")));
                }
            }

            return sb.ToString().TrimEnd();
        }

        // 8. Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new { a.AddressText, TownName = a.Town.Name, EmployeeCount = a.Employees.Count })
                .ToList();

            return string.Join(Environment.NewLine, addresses.Select(a => $"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees"));
        }

        // 9. Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new { e.FirstName, e.LastName, e.JobTitle, Projects = e.EmployeesProjects.Select(p => new { p.Project.Name }).OrderBy(p => p.Name).ToList() })
                .FirstOrDefault();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            sb.Append(string.Join(Environment.NewLine, employee.Projects.Select(p => p.Name)));

            return sb.ToString().TrimEnd();
        }

        //10. Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            EmployeeFirstName = e.FirstName,
                            EmployeeLastName = e.LastName,
                            EmployeeJobTitle = e.JobTitle
                        })
                        .ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.ManagerFirstName} {department.ManagerLastName}");
                sb.Append(string.Join(Environment.NewLine, department.Employees.Select(e => $"{e.EmployeeFirstName} {e.EmployeeLastName} - {e.EmployeeJobTitle}")));
            }

            return sb.ToString().TrimEnd();
        }

        // 11. Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    ProjectDescription = p.Description,
                    ProjectStartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.ProjectName}");
                sb.AppendLine($"{project.ProjectDescription}");
                sb.AppendLine($"{project.ProjectStartDate}");
            }

            return sb.ToString().TrimEnd();
        }

        // 12. Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            decimal salaryModifier = 1.12m;
            string[] departmentNames = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employeesForSalaryIncrease = context.Employees
                .Where(e => departmentNames.Contains(e.Department.Name))
                .ToList();

            foreach (var employee in employeesForSalaryIncrease)
            {
                employee.Salary *= salaryModifier;
            }

            context.SaveChanges();

            var employeesInfo = context.Employees
                .Where(e => departmentNames.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    EmployeeFirstName = e.FirstName,
                    EmployeeLastName = e.LastName,
                    EmployeeSalary = e.Salary
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employeesInfo)
            {
                sb.AppendLine($"{employee.EmployeeFirstName} {employee.EmployeeLastName} (${employee.EmployeeSalary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        // 13. Find Employees by First Name Starting with "Sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new { e.FirstName, e.LastName, e.JobTitle, e.Salary })
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        // 14. Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeesProjectsToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeesProjectsToDelete);

            var projectToDelete = context.Projects
                .Where(p => p.ProjectId == 2);

            context.Projects.RemoveRange(projectToDelete);

            context.SaveChanges();

            var projectsInfo = context.Projects
                .Take(10)
                .Select(p => new { ProjectName = p.Name })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var project in projectsInfo)
            {
                sb.AppendLine($"{project.ProjectName}");
            }

            return sb.ToString().TrimEnd();
        }

        // 15. Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            Address[] addressesToDelete = context.Addresses
                .Where(a => a.TownId == townToDelete.TownId)
                .ToArray();

            Employee[] employeesToRemoveAddressFrom = context.Employees
                .Where(e => addressesToDelete.Contains(e.Address))
                .ToArray();

            foreach (Employee employee in employeesToRemoveAddressFrom)
            {
                employee.AddressId = null;
            }

            context.Addresses.RemoveRange(addressesToDelete);
            context.Towns.Remove(townToDelete);

            context.SaveChanges();

            return $"{addressesToDelete.Count()} addresses in Seattle were deleted";
        }
    }
}
