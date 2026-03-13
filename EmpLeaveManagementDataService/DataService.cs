using System.Security.Principal;
using EmpLeaveManagementAppModel;

namespace EmpLeaveManagementDataService
{
    public class DataService
    {
        static List<Employee> Employees = new List<Employee>();
        static List<FiledLeave> FiledLeaves = new List<FiledLeave>();
        static List<AdminAccount> AdminAccounts = new List<AdminAccount>();

        // ----------------------------------------------------CONSTRUCTOR----------------------------------------------------
        public DataService() {
            AdminAccount adminAccount = new AdminAccount { AccountID = Guid.NewGuid(), Username = "dee", Password = "dee123" };
            Employee employee1 = new Employee { EmployeeID = Guid.NewGuid(), Name = "Rafael Antonio Dee" };
            Employee employee2 = new Employee { EmployeeID = Guid.NewGuid(), Name = "Indaleen Quinsayas" };
            AdminAccounts.Add(adminAccount);
            Employees.Add(employee1);
            Employees.Add(employee2);
        }


        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public void AddLeave(FiledLeave Leave)
        {
            FiledLeaves.Add(Leave);
        }
        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
        }
        public void AddAdmin(AdminAccount admin)
        {
            AdminAccounts.Add(admin);
        }


        // ----------------------------------------------------REMOVE FUNCTIONS----------------------------------------------------
        public void RemoveEmployee(Employee employee)
        {
            Employees.Remove(employee);
        }
        public void RemoveAdmin(AdminAccount admin)
        {
            AdminAccounts.Remove(admin);
        }


        // ----------------------------------------------------CHECK EXISTENCE FUNCTIONS----------------------------------------------------
        public bool EmployeeExists(string empName)
        {
            return Employees.Any(a => a.Name == empName);
        }
        public bool AdminExists(string username)
        {
            return AdminAccounts.Any(a => a.Username == username);
        }


        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------
        public Employee? GetEmployeeByName(string name)
        {
            return Employees.FirstOrDefault(a => a.Name == name);
        }
        public AdminAccount? GetAdminByUser(string user)
        {
            return AdminAccounts.FirstOrDefault(a => a.Username == user);
        }
        public AdminAccount? AccountGetByUsername(string username)
        {
            return AdminAccounts.FirstOrDefault(a => a.Username == username);
        }
        public Employee? GetById(Guid id)
        {
            return Employees.FirstOrDefault(a => a.EmployeeID == id);
        }


        // ----------------------------------------------------GET LISTS FUNCTIONS----------------------------------------------------
        public List<FiledLeave> GetLeaves()
        {
            return FiledLeaves;
        }
        public List<Employee> GetEmployees()
        {
            return Employees;
        }
        public List<AdminAccount> GetAdmins()
        {
            return AdminAccounts;
        }

    }
}
