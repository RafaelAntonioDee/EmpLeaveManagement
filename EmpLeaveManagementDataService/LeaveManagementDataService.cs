using System.Security.Principal;
using EmpLeaveManagementAppModel;

namespace EmpLeaveManagementDataService
{
    public class LeaveManagementDataService
    {
        ILeaveManagementDataService _dataService;
        public LeaveManagementDataService(ILeaveManagementDataService leaveManagementDataService) {
            _dataService = leaveManagementDataService;
        }


        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public void AddLeave(FiledLeave Leave)
        {
            _dataService.AddLeave(Leave);
        }
        public void AddEmployee(Employee employee)
        {
            _dataService.AddEmployee(employee);
        }
        public void AddAdmin(AdminAccount admin)
        {
            _dataService.AddAdmin(admin);
        }


        // ----------------------------------------------------REMOVE FUNCTIONS----------------------------------------------------
        public void RemoveEmployee(Employee employee)
        {
            _dataService.RemoveEmployee(employee);
        }
        public void RemoveAdmin(AdminAccount admin)
        {
            _dataService.RemoveAdmin(admin);
        }


        // ----------------------------------------------------CHECK EXISTENCE FUNCTIONS----------------------------------------------------
        public bool EmployeeExists(string empName)
        {
            return _dataService.EmployeeExists(empName);
        }
        public bool AdminExists(string username)
        {
            return _dataService.AdminExists(username);
        }


        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------
        public Employee? GetEmployeeByName(string name)
        {
            return _dataService.GetEmployeeByName(name);
        }
        public AdminAccount? GetAdminByUser(string user)
        {
            return _dataService.GetAdminByUser(user);
        }
        public AdminAccount? AccountGetByUsername(string username)
        {
            return _dataService.AccountGetByUsername(username);
        }
        public Employee? GetById(Guid id)
        {
            return _dataService.GetById(id);
        }


        // ----------------------------------------------------GET LISTS FUNCTIONS----------------------------------------------------
        public List<FiledLeave> GetLeaves()
        {
            return _dataService.GetLeaves();
        }
        public List<Employee> GetEmployees()
        {
            return _dataService.GetEmployees();
        }
        public List<AdminAccount> GetAdmins()
        {
            return _dataService.GetAdmins();
        }

    }
}
