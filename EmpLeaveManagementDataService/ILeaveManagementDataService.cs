using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmpLeaveManagementAppModel;

namespace EmpLeaveManagementDataService
{
    public interface ILeaveManagementDataService
    {
        void AddLeave(FiledLeave Leave);
        void AddEmployee(Employee employee);
        void AddAdmin(AdminAccount admin);
        void RemoveEmployee(Employee employee);
        void RemoveAdmin(AdminAccount admin);
        bool EmployeeExists(string empName);
        bool AdminExists(string username);
        Employee? GetEmployeeByName(string name);
        AdminAccount? GetAdminByUser(string user);
        AdminAccount? AccountGetByUsername(string username);
        Employee? GetById(Guid id);
        List<FiledLeave> GetLeaves();
        List<Employee> GetEmployees();
        List<AdminAccount> GetAdmins();
    }
}
