using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpLeaveManagementAppModel
{
    public class FiledLeave
    {
        public Guid EmployeeID { get; set; }
        public string Name { get; set; }
        public string TypeOfLeaves { get; set; }
        public int DaysOfLeaves { get; set; }
        public string DateOfLeave { get; set; }
    }
}
