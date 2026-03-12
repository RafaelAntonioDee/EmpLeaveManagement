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

        public Employee Register(Employee employee)
        {
            if (!EmpDataService.EmployeeExists(employee.Name))
            {
                EmpDataService.AddEmployee(employee);
            }
            return employee;
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

        public bool isNameExist(string name)
        {
            if (EmpDataService.GetByName(name) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Employee? GetByName(string name)
        {
            return EmpDataService.GetByName(name);
        }
    }
}
