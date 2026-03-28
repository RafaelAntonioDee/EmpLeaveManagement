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
        void AddLeave(FiledLeave Leave, Employee emp);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee, string newPass, string newPosition);
        void RemoveEmployee(Employee employee);
        int GetNewLeaveID();
        int GetNewEmployeeID();

        bool EmployeeExists(int id);
        Employee? GetEmployee(int id);
        EmployeeLeaveData? GetEmployeeLeaveData(int id);
        List<FiledLeave> GetLeaves();
        List<Employee> GetEmployees();
        void CalculateAvailableDays(int empID, string TypeOfLeave, int Days);
    }
}
