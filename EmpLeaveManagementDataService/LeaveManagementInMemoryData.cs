using System.Security.Principal;
using EmpLeaveManagementAppModel;

namespace EmpLeaveManagementDataService
{
    public class LeaveManagementInMemoryData : ILeaveManagementDataService
    {
        static List<Employee> Employees = new List<Employee>();
        static List<EmployeeLeaveData> EmployeesLeaveData = new List<EmployeeLeaveData>();
        static List<FiledLeave> FiledLeaves = new List<FiledLeave>();

        // ----------------------------------------------------CONSTRUCTOR----------------------------------------------------
        public LeaveManagementInMemoryData()
        {
            Employee employee1 = new Employee { EmployeeID = 100000, FirstName = "Rafael Antonio", LastName = "Dee", Password = "dee123", Position = "Admin" };
            Employee employee2 = new Employee { EmployeeID = 100001, FirstName = "Indaleen", LastName = "Quinsayas", Password = "123", Position = "Supervisor" };
            Employee employee3 = new Employee { EmployeeID = 100002, FirstName = "John", LastName = "Doe", Password = "123", Position = "Sales" };

            AddEmployee(employee1);
            AddEmployee(employee2);
            AddEmployee(employee3);
        }

        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public void AddLeave(FiledLeave Leave, Employee emp)
        {
            FiledLeaves.Add(Leave);
        }

        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
            EmployeeLeaveData data = new EmployeeLeaveData { EmployeeID = employee.EmployeeID };
            EmployeesLeaveData.Add(data);
        }

        // ----------------------------------------------------REMOVE FUNCTIONS----------------------------------------------------
        public void RemoveEmployee(Employee employee)
        {
            Employees.Remove(employee);
            EmployeeLeaveData empleavedate = GetEmployeeLeaveData(employee.EmployeeID);
            EmployeesLeaveData.Remove(empleavedate);
        }

        // ----------------------------------------------------UPDATE FUNCTIONS----------------------------------------------------
        public void UpdateEmployee(Employee empUpdate, string newPass, string newPosition)
        {
            empUpdate.Password = newPass;
            empUpdate.Position = newPosition;   
        }

        // ----------------------------------------------------CHECK EXISTENCE FUNCTIONS----------------------------------------------------
        public bool EmployeeExists(int ID)
        {
            return Employees.Any(a => a.EmployeeID == ID);
        }

        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------

        public Employee? GetEmployee(int id)
        {
            return Employees.FirstOrDefault(a => a.EmployeeID == id);
        }
        public EmployeeLeaveData? GetEmployeeLeaveData(int id)
        {
            return EmployeesLeaveData.FirstOrDefault(a => a.EmployeeID == id);
        }
        public int GetNewLeaveID()
        {
            if (FiledLeaves.Count == 0)
            {
                return 100000;
            }
            else
            {
                int latest = FiledLeaves.Max(e => e.LeaveID)+1;
                return latest;
            }
        }
        public int GetNewEmployeeID()
        {
            if (Employees.Count == 0)
            {
                return 100000;
            }
            else
            {
                int latest = Employees.Max(e => e.EmployeeID)+1;
                return latest;
            }

        }
        public void CalculateAvailableDays(int empID, string TypeOfLeave, int Days)
        {
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
    }
}
