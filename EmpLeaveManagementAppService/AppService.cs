using System.Security.Principal;
using EmpLeaveManagementAppModel;
using EmpLeaveManagementDataService;

namespace EmpLeaveManagementAppService
{
    public class AppService
    {
        DataService EmpDataService = new DataService();

        // ----------------------------------------------------FILE LEAVE FUNCTIONS----------------------------------------------------
        public void CalculateAvailableLeaveDays(String EmployeeName, String TypeOfLeave, int Days)
        {
            var Emp = EmpDataService.GetEmployeeByName(EmployeeName);

            switch (TypeOfLeave)
            {
                case "Maternity Leave":

                    Emp.MaternityLeave -= Days;
                    break;
                case "Paternity Leave":
                    Emp.PaternityLeave -= Days;
                    break;
                case "Sick Leave":
                    Emp.SickLeave -= Days;
                    break;
                case "Vacation Leave":
                    Emp.VacationLeave -= Days;
                    break;
            }
        }
        public void RecordLeave(FiledLeave Leave)
        {
            EmpDataService.AddLeave(Leave);
        }
        public int checkDaysOfLeaveAvailable(string LeaveType, Employee emp)
        {
            while (true)
            {
                switch (LeaveType)
                {
                    case "Maternity Leave":
                        return emp.MaternityLeave;
                    case "Paternity Leave":
                        return emp.PaternityLeave;
                    case "Sick Leave":
                        return emp.SickLeave;
                    case "Vacation Leave":
                        return emp.VacationLeave;
                }
            }
        }


        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------
        public List<FiledLeave> GetLeaves()
        {
            return EmpDataService.GetLeaves();
        }
        public List<Employee> GetEmployees()
        {
            return EmpDataService.GetEmployees();
        }
        public List<AdminAccount> getAdmins()
        {
            return EmpDataService.GetAdmins();
        }
        public Employee GetEmployee(string EmpName)
        {
            return EmpDataService.GetEmployeeByName(EmpName);
        }


        // ----------------------------------------------------CHECK FUNCTIONS----------------------------------------------------
        public bool checkEmployee(string EmpName)
        {
            return EmpDataService.EmployeeExists(EmpName);
        }
        public bool checkAdmin(string username)
        {
            return EmpDataService.AdminExists(username);
        }


        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public void AddEmployee(string EmpName)
        {
            Employee newEmployee = new Employee { EmployeeID = Guid.NewGuid(), Name = EmpName };
            EmpDataService.AddEmployee(newEmployee);
        }
        public void AddAdmin(string user, string pass)
        {
            AdminAccount newAdmin = new AdminAccount { AccountID = Guid.NewGuid(), Username = user, Password = pass };
            EmpDataService.AddAdmin(newAdmin);
        }


        // ----------------------------------------------------REMOVE FUNCTIONS----------------------------------------------------
        public void RemoveEmployee(string EmpName) {
            Employee emp = EmpDataService.GetEmployeeByName(EmpName);
            EmpDataService.RemoveEmployee(emp);
        }
        public void RemoveAdmin(string user)
        {
            AdminAccount admin = EmpDataService.GetAdminByUser(user);
            EmpDataService.RemoveAdmin(admin);
        }


        // ----------------------------------------------------LOGIN FUNCTION----------------------------------------------------
        public bool Authenticate(string username, string password)
        {
            var account = EmpDataService.AccountGetByUsername(username);

            if (account == null)
                return false;

            return account.Password == password;
        }
    }
}
