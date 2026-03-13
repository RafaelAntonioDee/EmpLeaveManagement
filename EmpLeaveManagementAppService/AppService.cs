using System.Security.Principal;
using EmpLeaveManagementAppModel;
using EmpLeaveManagementDataService;

namespace EmpLeaveManagementAppService
{
    public class AppService
    {
        DataService EmpDataService = new DataService();

        public void RecordLeave(FiledLeave Leave)
        {
            EmpDataService.AddLeave(Leave);
        }

        public void CalculateAvailableLeaveDays(String EmployeeName, String TypeOfLeave, int Days)
        {
            var Emp = EmpDataService.GetByName(EmployeeName);

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

        public List<FiledLeave> GetLeaves()
        {
            return EmpDataService.GetLeaves();

        }
        public List<Employee> GetEmployees()
        {
            return EmpDataService.GetEmployees();

        }

        public Employee? GetByName(string name)
        {
            return EmpDataService.GetByName(name);
        }

        public Employee GetEmployee(string EmpName)
        {
            Employee Emp;
            if (EmpDataService.EmployeeExists(EmpName))
            {
                return Emp = EmpDataService.GetByName(EmpName);
            }
            else
            {
                Employee newEmployee = new Employee { EmployeeID = Guid.NewGuid(), Name = EmpName };
                EmpDataService.AddEmployee(newEmployee);
                return Emp = newEmployee;
            }
        }

    }
}
