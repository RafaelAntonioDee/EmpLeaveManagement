namespace EmpLeaveManagementAppModel
{
    public class Employee
    {
        public Guid EmployeeID { get; set; }
        public string Name { get; set; }
        public int MaternityLeave { get; set; } = 105;
        public int PaternityLeave { get; set; } = 7;
        public int VacationLeave { get; set; } = 15;
        public int SickLeave { get; set; } = 15;
    }
}
