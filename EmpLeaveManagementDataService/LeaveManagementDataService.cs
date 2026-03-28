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
        public void AddLeave(FiledLeave Leave, Employee emp)
        {
            _dataService.AddLeave(Leave, emp);
        }
        public void AddEmployee(Employee employee)
        {
            _dataService.AddEmployee(employee);
        }

        // ----------------------------------------------------UPDATE FUNCTIONS----------------------------------------------------

        public void UpdateEmployee(Employee empUpdate, string newPass, string newPosition)
        {
            _dataService.UpdateEmployee(empUpdate, newPass, newPosition);
        }

        // ----------------------------------------------------REMOVE FUNCTIONS----------------------------------------------------
        public void RemoveEmployee(Employee employee)
        {
            _dataService.RemoveEmployee(employee);
        }



        // ----------------------------------------------------CHECK EXISTENCE FUNCTIONS----------------------------------------------------
        public bool EmployeeExists(int ID)
        {
            return _dataService.EmployeeExists(ID);
        }



        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------
        public Employee? GetEmployee(int id)
        {
            return _dataService.GetEmployee(id);
        }
        public EmployeeLeaveData? GetEmployeeLeaveData(int id)
        {
            return _dataService.GetEmployeeLeaveData(id);
        }
        public int GetNewLeaveID()
        {
            return _dataService.GetNewLeaveID();
        }
        public int GetNewEmployeeID()
        {
            return _dataService.GetNewEmployeeID();
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

        public void CalculateAvailableDays(int empID, string TypeOfLeave, int Days)
        {
            _dataService.CalculateAvailableDays(empID, TypeOfLeave, Days);
        }
    }
}
