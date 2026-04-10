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
        static List<EmployeeLeaveData> EmployeesLeaveData = new List<EmployeeLeaveData>();

        private string _jsonFileName;

        public LeaveManagementJsonData()
        {
            _jsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\DataStorage.json");
            PopulateJsonFile();
        }

        private void PopulateJsonFile()
        {
            RetrieveDataFromJsonFile();

            if (Employees.Count <= 0)
            {
                Employee employee1 = new Employee { EmployeeID = 100000, FirstName = "Rafael Antonio", LastName = "Dee", Password = "dee123", Position = "Admin" };
                Employee employee2 = new Employee { EmployeeID = 100001, FirstName = "Indaleen", LastName = "Quinsayas", Password = "123", Position = "Supervisor" };
                Employee employee3 = new Employee { EmployeeID = 100002, FirstName = "John", LastName = "Doe", Password = "123", Position = "Sales" };

                AddEmployee(employee1);
                AddEmployee(employee2);
                AddEmployee(employee3);
            }
            SaveDataToJsonFile();
        }
        public class Database
        {
            public List<Employee> Employees { get; set; }
            public List<FiledLeave> FiledLeaves { get; set; }
            public List<EmployeeLeaveData> EmployeesLeaveData { get; set; }
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
                        EmployeesLeaveData = EmployeesLeaveData
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
                EmployeesLeaveData = new List<EmployeeLeaveData>();
                return;
            }

            using (var jsonFileReader = File.OpenText(_jsonFileName))
            {
                string json = jsonFileReader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(json))
                {
                    Employees = new List<Employee>();
                    FiledLeaves = new List<FiledLeave>();
                    EmployeesLeaveData = new List<EmployeeLeaveData>();
                    return;
                }

                var db = JsonSerializer.Deserialize<Database>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Employees = db?.Employees ?? new List<Employee>();
                FiledLeaves = db?.FiledLeaves ?? new List<FiledLeave>();
                EmployeesLeaveData = db?.EmployeesLeaveData ?? new List<EmployeeLeaveData>();
            }
        }

        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public void AddLeave(FiledLeave Leave, Employee emp)
        {
            FiledLeaves.Add(Leave);
            SaveDataToJsonFile();
        }
        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
            EmployeeLeaveData data = new EmployeeLeaveData { EmployeeID = employee.EmployeeID };
            EmployeesLeaveData.Add(data);
            SaveDataToJsonFile();
        }

        // ----------------------------------------------------UPDATE FUNCTIONS----------------------------------------------------
        public void UpdateEmployee(Employee empUpdate, string newPass, string newPosition)
        {
            empUpdate = GetEmployee(empUpdate.EmployeeID);
            empUpdate.Password = newPass;
            empUpdate.Position = newPosition;
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
            }
            SaveDataToJsonFile();
            RetrieveDataFromJsonFile();

            EmployeeLeaveData empdata = GetEmployeeLeaveData(employee.EmployeeID);
            var existingdata = EmployeesLeaveData.FirstOrDefault(e => e.EmployeeID == empdata.EmployeeID);

            if (existing != null)
            {
                EmployeesLeaveData.Remove(existingdata);
            }

            SaveDataToJsonFile();

        }

        // ----------------------------------------------------CHECK EXISTENCE FUNCTIONS----------------------------------------------------
        public bool EmployeeExists(int id)
        {
            RetrieveDataFromJsonFile();
            return Employees.Any(a => a.EmployeeID == id);
        }


        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------
        public Employee? GetEmployee(int id)
        {
            RetrieveDataFromJsonFile();
            return Employees.FirstOrDefault(a => a.EmployeeID == id);
        }
        public EmployeeLeaveData? GetEmployeeLeaveData(int id)
        {
            RetrieveDataFromJsonFile();
            return EmployeesLeaveData.FirstOrDefault(a => a.EmployeeID == id);
        }
        public int GetNewLeaveID()
        {
            RetrieveDataFromJsonFile();
            if (FiledLeaves.Count == 0)
            {
                return 100000;
            }
            else
            {
                int latest = FiledLeaves.Max(e => e.LeaveID) + 1;
                return latest;
            }
        }
        public int GetNewEmployeeID()
        {
            RetrieveDataFromJsonFile();
            if (Employees.Count == 0)
            {
                return 100000;
            }
            else
            {
                int latest = Employees.Max(e => e.EmployeeID) + 1;
                return latest;
            }

        }

        public void CalculateAvailableDays(int empID, string TypeOfLeave, int Days)
        {
            RetrieveDataFromJsonFile();
            EmployeeLeaveData empLeaveData = GetEmployeeLeaveData(empID);

            switch (TypeOfLeave)
            {
                case "Maternity Leave":
                    empLeaveData.MaternityLeave -= Days;
                    break;
                case "Paternity Leave":
                    empLeaveData.PaternityLeave -= Days;
                    break;
                case "Sick Leave":
                    empLeaveData.SickLeave -= Days;
                    break;
                case "Vacation Leave":
                    empLeaveData.VacationLeave -= Days;
                    break;
            }
            SaveDataToJsonFile();

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

    }
}