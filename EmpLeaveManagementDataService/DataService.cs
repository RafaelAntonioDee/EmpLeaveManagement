using System.Security.Principal;
using EmpLeaveManagementAppModel;

namespace EmpLeaveManagementDataService
{
    public class DataService
    {
        static List<Employee> Employees = new List<Employee>();
        static List<FiledLeave> FiledLeaves = new List<FiledLeave>();


        public DataService() {
        }

        public void AddLeave(FiledLeave Leave)
        {
            FiledLeaves.Add(Leave);
        }

        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
        }

        public bool EmployeeExists(string empName)
        {
            return Employees.Any(a => a.Name == empName);
        }
        public Employee? GetByName(string name)
        {
            return Employees.FirstOrDefault(a => a.Name == name);
        }

        public Employee? GetById(Guid id)
        {
            return Employees.FirstOrDefault(a => a.EmployeeID == id);
        }

        public List<FiledLeave> GetLeaves()
        {
            return FiledLeaves;
        }
        public List<Employee> GetEmployees()
        {
            return Employees;
        }

    }
}
