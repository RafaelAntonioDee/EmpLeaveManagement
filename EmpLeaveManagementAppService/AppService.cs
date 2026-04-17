using System.Collections.Generic;
using System.Security.Principal;
using EmpLeaveManagementAppModel;
using EmpLeaveManagementDataService;

namespace EmpLeaveManagementAppService
{
    public class AppService
    {
        LeaveManagementDataService EmpDataService = new LeaveManagementDataService(new LeaveManagementInMemoryData());

        // ----------------------------------------------------FILE LEAVE FUNCTIONS----------------------------------------------------
        public void CalculateAvailableLeaveDays(int empID, String TypeOfLeave, int Days)
        {
            EmpDataService.CalculateAvailableDays(empID, TypeOfLeave, Days);

        }
        public void RecordLeave(Employee emp, string LeaveType, int LeaveDays, string LeaveDate)
        {
            FiledLeave newLeave = new FiledLeave { LeaveID = EmpDataService.GetNewLeaveID(), EmployeeID = emp.EmployeeID, Name = (emp.FirstName + " " + emp.LastName), TypeOfLeave = LeaveType, DaysOfLeave = LeaveDays, DateOfLeave = LeaveDate };

            EmpDataService.AddLeave(newLeave, emp);
        }
        public int checkDaysOfLeaveAvailable(string LeaveType, EmployeeLeaveData EmpLeaveData)
        {
            while (true)
            {
                switch (LeaveType)
                {
                    case "Maternity Leave":
                        return EmpLeaveData.MaternityLeave;
                    case "Paternity Leave":
                        return EmpLeaveData.PaternityLeave;
                    case "Sick Leave":
                        return EmpLeaveData.SickLeave;
                    case "Vacation Leave":
                        return EmpLeaveData.VacationLeave;
                }
            }
        }
        public bool isLeaveAvailable(int EmpID)
        {
            EmployeeLeaveData EmpLeaveData = GetEmployeeLeaveData(EmpID);
            if (EmpLeaveData.MaternityLeave == 0 && EmpLeaveData.PaternityLeave == 0 && EmpLeaveData.SickLeave == 0 && EmpLeaveData.VacationLeave == 0)
            {
                return false;
            }
            else
            {
                return true;
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

        public Employee GetEmployee(int EmpID)
        {
            return EmpDataService.GetEmployee(EmpID);
        }
        public EmployeeLeaveData GetEmployeeLeaveData(int EmpID)
        {
            return EmpDataService.GetEmployeeLeaveData(EmpID);
        }

        public int GetAdminCount()
        {
            var employees = GetEmployees();

            int adminCount = employees.Count(e => e.Position == "Admin");

            return adminCount;
        }


        // ----------------------------------------------------CHECK FUNCTIONS----------------------------------------------------
        public bool checkEmployee(int empID)
        {
            return EmpDataService.EmployeeExists(empID);
        }

        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public int AddEmployee(string fName, string lName, string Password, string Position)
        {
            Employee newEmployee = new Employee { EmployeeID = EmpDataService.GetNewEmployeeID(), FirstName = fName, LastName = lName, Password = Password, Position = Position };
            EmpDataService.AddEmployee(newEmployee);
            return newEmployee.EmployeeID;
        }

        // ----------------------------------------------------UPDATE FUNCTIONS----------------------------------------------------
        public void UpdateEmployee(Employee empUpdate, string newPass, string newPosition)
        {
            EmpDataService.UpdateEmployee(empUpdate, newPass, newPosition);
        }
        
        public bool UpdatedOwnAccount(int UpdatingEmployeeID, int EmployeeID, string newPosition)
        {
            if (newPosition != "Admin" && UpdatingEmployeeID == EmployeeID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // ----------------------------------------------------REMOVE FUNCTIONS----------------------------------------------------
        public void RemoveEmployee(int ID)
        {
            Employee emp = EmpDataService.GetEmployee(ID);
            EmpDataService.RemoveEmployee(emp);
        }

        public bool RemovedOwnAccount(int RemovingEmployeeID, int EmployeeID)
        {
            if (RemovingEmployeeID == EmployeeID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AdminCountCheck(int ID)
        {
            Employee RemovingEmp = GetEmployee(ID);

            if (RemovingEmp.Position == "Admin" && GetAdminCount() <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // ----------------------------------------------------LOGIN FUNCTION----------------------------------------------------
        public bool Authenticate(int empID, string password)
        {
            var account = EmpDataService.GetEmployee(empID);

            if (account == null)
                return false;

            return account.Password == password;
        }
        public bool isAdmin(int empID)
        {
            var account = EmpDataService.GetEmployee(empID);

            if (account == null)
                return false;

            return account.Position == "Admin";
        }
    }
}
