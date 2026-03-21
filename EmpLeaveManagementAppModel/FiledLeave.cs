using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpLeaveManagementAppModel
{
    public class FiledLeave
    {
        public Guid LeaveID { get; set; }
        public Guid EmployeeID { get; set; }
        public string Name { get; set; }
        public string TypeOfLeave { get; set; }
        public int DaysOfLeave { get; set; }
        public string DateOfLeave { get; set; }
    }
}
