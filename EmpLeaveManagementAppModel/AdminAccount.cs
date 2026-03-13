using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpLeaveManagementAppModel
{
    public class AdminAccount
    {
        public Guid AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
