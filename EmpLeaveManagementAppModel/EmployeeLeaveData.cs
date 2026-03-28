using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpLeaveManagementAppModel
{
    public class EmployeeLeaveData
    {
        public int EmployeeID { get; set; }
        public int MaternityLeave { get; set; } = 105;
        public int PaternityLeave { get; set; } = 7;
        public int VacationLeave { get; set; } = 15;
        public int SickLeave { get; set; } = 15;
    }
}
