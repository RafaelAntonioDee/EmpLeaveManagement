using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpLeaveManagementAppModel
{
    public class FiledLeave
    {
        public int LeaveID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string TypeOfLeave { get; set; }
        public int DaysOfLeave { get; set; }
        public string DateOfLeave { get; set; }
    }
}
