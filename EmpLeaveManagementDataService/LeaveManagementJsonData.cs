using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EmpLeaveManagementAppModel;

namespace EmpLeaveManagementDataService
{
    public class LeaveManagementJsonData : ILeaveManagementDataService
    {
        static List<Employee> Employees = new List<Employee>();
        static List<FiledLeave> FiledLeaves = new List<FiledLeave>();
        static List<AdminAccount> AdminAccounts = new List<AdminAccount>();

        private string _jsonFileName;

        public LeaveManagementJsonData()
        {
            _jsonFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/DataStorage.json";

            PopulateJsonFile();
        }

        private void PopulateJsonFile()
        {
            RetrieveDataFromJsonFile();

            if (AdminAccounts.Count <= 0)
            {
                AdminAccounts.Add(new AdminAccount { AccountID = Guid.NewGuid(), Username = "dee", Password = "dee123" });
            }
            if (Employees.Count <= 0)
            {
                Employees.Add(new Employee { EmployeeID = Guid.NewGuid(), Name = "Rafael Antonio Dee" });
                Employees.Add(new Employee { EmployeeID = Guid.NewGuid(), Name = "Indaleen Quinsayas" });
            }
            SaveDataToJsonFile();
        }
        public class Database
        {
            public List<Employee> Employees { get; set; }
            public List<FiledLeave> FiledLeaves { get; set; }
            public List<AdminAccount> AdminAccounts { get; set; }
        }
        private void SaveDataToJsonFile()
        {
            using (var outputStream = File.Create(_jsonFileName))
            {
                JsonSerializer.Serialize<Database>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    { SkipValidation = true, Indented = true }),

                    new Database
                    {
                        Employees = Employees,
                        FiledLeaves = FiledLeaves,
                        AdminAccounts = AdminAccounts
                    }
                );
            }
        }

        private void RetrieveDataFromJsonFile()
        {
            if (!File.Exists(_jsonFileName))
            {
                Employees = new List<Employee>();
                FiledLeaves = new List<FiledLeave>();
                AdminAccounts = new List<AdminAccount>();
                return;
            }

            using (var jsonFileReader = File.OpenText(_jsonFileName))
            {
                string json = jsonFileReader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(json))
                {
                    Employees = new List<Employee>();
                    FiledLeaves = new List<FiledLeave>();
                    AdminAccounts = new List<AdminAccount>();
                    return;
                }

                var db = JsonSerializer.Deserialize<Database>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Employees = db?.Employees ?? new List<Employee>();
                FiledLeaves = db?.FiledLeaves ?? new List<FiledLeave>();
                AdminAccounts = db?.AdminAccounts ?? new List<AdminAccount>();
            }
        }

        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public void AddLeave(FiledLeave Leave)
        {
            FiledLeaves.Add(Leave);
            SaveDataToJsonFile();
        }
        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
            SaveDataToJsonFile();
        }
        public void AddAdmin(AdminAccount admin)
        {
            AdminAccounts.Add(admin);
            SaveDataToJsonFile();
        }


        // ----------------------------------------------------REMOVE FUNCTIONS----------------------------------------------------
        public void RemoveEmployee(Employee employee)
        {
            RetrieveDataFromJsonFile();

            var existing = Employees.FirstOrDefault(e => e.EmployeeID == employee.EmployeeID);

            if (existing != null)
            {
                Employees.Remove(existing);
                SaveDataToJsonFile();
            }
        }
        public void RemoveAdmin(AdminAccount admin)
        {
            RetrieveDataFromJsonFile();

            var existing = AdminAccounts.FirstOrDefault(a => a.AccountID == admin.AccountID);

            if (existing != null)
            {
                AdminAccounts.Remove(existing);
                SaveDataToJsonFile();
            }
        }


        // ----------------------------------------------------CHECK EXISTENCE FUNCTIONS----------------------------------------------------
        public bool EmployeeExists(string empName)
        {
            RetrieveDataFromJsonFile();
            return Employees.Any(a => a.Name == empName);
        }
        public bool AdminExists(string username)
        {
            RetrieveDataFromJsonFile();
            return AdminAccounts.Any(a => a.Username == username);
        }


        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------
        public Employee? GetEmployeeByName(string name)
        {
            RetrieveDataFromJsonFile();
            return Employees.FirstOrDefault(a => a.Name == name);
        }
        public AdminAccount? GetAdminByUser(string user)
        {
            RetrieveDataFromJsonFile();
            return AdminAccounts.FirstOrDefault(a => a.Username == user);
        }
        public AdminAccount? AccountGetByUsername(string username)
        {
            RetrieveDataFromJsonFile();
            return AdminAccounts.FirstOrDefault(a => a.Username == username);
        }
        public Employee? GetById(Guid id)
        {
            RetrieveDataFromJsonFile();
            return Employees.FirstOrDefault(a => a.EmployeeID == id);
        }


        // ----------------------------------------------------GET LISTS FUNCTIONS----------------------------------------------------
        public List<FiledLeave> GetLeaves()
        {
            RetrieveDataFromJsonFile();
            return FiledLeaves;
        }
        public List<Employee> GetEmployees()
        {
            RetrieveDataFromJsonFile();
            return Employees;
        }
        public List<AdminAccount> GetAdmins()
        {
            RetrieveDataFromJsonFile();
            return AdminAccounts;
        }
    }
}