using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using EmpLeaveManagementAppModel;
using EmpLeaveManagementAppService;

internal class EmpLeaveManagement
{
    static AppService EmployeeAppService = new AppService();
    static Employee emp;

    // ----------------------------------------------------MAIN----------------------------------------------------
    static void Main(string[] args)
    {
        bool continueSystem = true;
        while (continueSystem)
        {
            int Choice = checkMenuChoice();
            if (Choice == 1)
            {
                if (Login())
                {
                    if (EmployeeAppService.isAdmin(emp.EmployeeID))
                    {
                        if (!AdminDashboard())
                        {
                            continue;
                        }
                    }
                    else
                    {
                        EmployeeDashboard();
                    }
                }


            }
            else
            {
                Console.WriteLine("System Closed.");

                Environment.Exit(0);
            }
        }
    }


    // ----------------------------------------------------MENU FUNCTIONS----------------------------------------------------
    static bool Login()
    {
        while (true)
        {


            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("======================== LOGIN ========================");
                Console.Write("Employee ID: ");
                int ID = Convert.ToInt32(Console.ReadLine());
                Console.Write("Password: ");
                String pass = Console.ReadLine();
                Console.WriteLine();

                bool isMatched = EmployeeAppService.Authenticate(ID, pass);

                if (isMatched)
                {
                    emp = EmployeeAppService.GetEmployee(ID);
                    return true;
                }
                else
                {
                    Console.WriteLine("ID does not exist or incorrect password.");
                }
            }
            Console.Write("Would you like to login again? (y/n): ");
            char YorN = Console.ReadLine()[0];

            if (YorN == 'n')
            {
                return false;
            }
        }
    }

    static int checkMenuChoice()
    {
        while (true)
        {
            Console.WriteLine("======================== EMPLOYEE LEAVE MANAGEMENT ========================");
            Console.WriteLine("[1] Login \n[2] Close System");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();

            if ((Choice <= 0) || Choice > 2)
            {
                Console.WriteLine("Option does not exist, choose again.\n");
            }
            else
            {
                return Choice;
            }
        }
    }

    static void EmployeeDashboard()
    {
        while (true)
        {
            Console.WriteLine("======================== EMPLOYEE DASHBOARD ========================\n");
            Console.WriteLine("Welcome " + emp.FirstName + " " + emp.LastName + "!\n");
            Console.WriteLine("[1] File Leave \n[2] Check Leaves \n[3] Logout");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();

            if (Choice == 1)
            {
                EmployeeFileLeave();
            }
            else if (Choice == 2)
            {
                EmployeeCheckLeaves();
            }
            else if (Choice == 3)
            {
                break;
            }
            else
            {
                Console.WriteLine("Option does not exist, choose again.\n");
            }
        }
    }

    static bool AdminDashboard()
    {
        while (true)
        {
            Console.WriteLine("======================== ADMIN DASHBOARD ========================\n");
            Console.WriteLine("Welcome Admin " + emp.FirstName + " " + emp.LastName + "!\n");

            Console.WriteLine("[1] File Leave \n[2] Check Leaves \n[3] View Employees Leave History \n[4] Manage Employees \n[5] Logout");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();

            if (Choice == 1)
            {
                EmployeeFileLeave();
            }
            else if (Choice == 2)
            {
                EmployeeCheckLeaves();
            }
            else if (Choice == 3)
            {
                showFiledLeaves();
            }
            else if (Choice == 4)
            {
                if (!manageEmployees())
                {
                    return false;
                }
            }
            else if (Choice == 5)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Option does not exist, choose again.\n");
            }
        }
    }

    // ----------------------------------------------------EMPLOYEE FUNCTIONS----------------------------------------------------
    static void EmployeeFileLeave()
    {
        if (EmployeeAppService.isLeaveAvailable(emp.EmployeeID))
        {
            Console.WriteLine("======================== FILE LEAVE ========================");

            String LeaveType = setLeaveType(emp.EmployeeID);
            int LeaveDays = setDaysOfLeave(LeaveType, emp.EmployeeID);
            String LeaveDate = setDateOfLeave();

            EmployeeAppService.CalculateAvailableLeaveDays(emp.EmployeeID, LeaveType, LeaveDays);

            EmployeeAppService.RecordLeave(emp, LeaveType, LeaveDays, LeaveDate);
        }
        else
        {
            Console.WriteLine("You have reached the maximum leaves available per year");
        }
    }

    static void EmployeeCheckLeaves()
    {
        var leaves = EmployeeAppService.GetLeaves();
        int leaveCount = 0;

        if (leaves.Count > 0)
        {

            foreach (var leave in leaves)
            { 
                if (leave.EmployeeID == emp.EmployeeID)
                {
                    if (leaveCount == 0)
                    {
                        Console.WriteLine("Leave ID\tType of Leave\t\tDays\t\tDate \n");
                    }
  
                    Console.WriteLine($"{leave.LeaveID}\t|\t{leave.TypeOfLeave}\t|\t{leave.DaysOfLeave}\t|\t{leave.DateOfLeave}");
                    leaveCount++;
                }
                if (leaveCount == 0)
                {
                    Console.WriteLine("No data yet.");
                    break;

                }
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("No data yet.\n");
        }
    }

    static String setLeaveType(int empID)
    {

        while (true)
        {
            EmployeeLeaveData EmpLeaveData = EmployeeAppService.GetEmployeeLeaveData(empID);
            Console.WriteLine("Input\t\tType of Leave\t\tAvailable Days \n" +
            $"[1]\t|\tMaternity Leave\t|\t{EmpLeaveData.MaternityLeave} \n" +
            $"[2]\t|\tPaternity Leave\t|\t{EmpLeaveData.PaternityLeave} \n" +
            $"[3]\t|\tSick Leave\t|\t{EmpLeaveData.SickLeave} \n" +
            $"[4]\t|\tVacation Leave\t|\t{EmpLeaveData.VacationLeave} ");

            Console.Write("Input: ");
            int option = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();

            string LeaveType = "";
            switch (option)
            {
                case 1:
                    LeaveType = "Maternity Leave";
                    break;
                case 2:
                    LeaveType = "Paternity Leave";
                    break;
                case 3:
                    LeaveType = "Sick Leave";
                    break;
                case 4:
                    LeaveType = "Vacation Leave";
                    break;
                default:
                    Console.WriteLine("Option does not exist, choose again.\n");
                    break;
            }
            if (LeaveType != "")
            {
                if (EmployeeAppService.checkDaysOfLeaveAvailable(LeaveType, EmpLeaveData) > 0)
                {
                    return LeaveType;
                }
                else
                {
                    Console.WriteLine($"There is no available days left for {LeaveType}, Choose again.\n");
                }
            }
        }
    }

    static int setDaysOfLeave(string LeaveType, int empID)
    {
        while (true)
        {
            int DaysofLeave = 0;

            if (LeaveType == "Maternity Leave")
            {
                DaysofLeave = 105;
            }
            else
            {
                Console.Write("Input Days of Leave: ");
                DaysofLeave = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine();
            }

            EmployeeLeaveData EmpLeaveData = EmployeeAppService.GetEmployeeLeaveData(empID);
            int LeaveTypeAvailable = EmployeeAppService.checkDaysOfLeaveAvailable(LeaveType, EmpLeaveData);

            if (DaysofLeave <= 0)
            {
                Console.WriteLine($"Please input a valid amount.");
                Console.WriteLine();
            }
            else if (DaysofLeave > LeaveTypeAvailable)
            {
                Console.WriteLine($"{DaysofLeave} day(s) exceeds {LeaveTypeAvailable} day(s) of available leaves.");
                Console.WriteLine();
            }
            else
            {
                return DaysofLeave;
            }
        }
    }

    static string setDateOfLeave()
    {
        Console.Write("Date of Leave: ");
        String LeaveDate = Console.ReadLine();
        Console.WriteLine();
        return LeaveDate;
    }


    // ----------------------------------------------------ADMIN FUNCTIONS----------------------------------------------------
    static bool manageEmployees()
    {
        while (true)
        {
            Console.WriteLine("======================== MANAGE EMPLOYEES ========================");
            Console.WriteLine("[1] View Employee List\n[2] Add Employee\n[3] Update Employee\n[4] Remove Employee\n[5] Exit");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine();

            if (Choice == 1)
            {
                showEmployeeList();
            }
            else if (Choice == 2)
            {
                addEmployees();
            }
            else if (Choice == 3)
            {
                if (!updateEmployee())
                {
                    return false;
                }
            }
            else if (Choice == 4)
            {
                if (!removeEmployees())
                {
                    return false;
                }
            }
            else if (Choice == 5)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Option does not exist, choose again.\n");
            }


        }
    }

    static bool updateEmployee()
    {
        Console.Write("Update Employee. Input ID: ");
        int ID = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();

        if (EmployeeAppService.checkEmployee(ID))
        {
            Employee empUpdate = EmployeeAppService.GetEmployee(ID);

            Console.WriteLine("Employee " + empUpdate.EmployeeID + ": " + empUpdate.FirstName + " " + empUpdate.LastName);
            Console.WriteLine();

            Console.Write("Update Password: ");
            string newPass = Console.ReadLine();

            string newPosition = empUpdate.Position;
            if (!EmployeeAppService.AdminCountCheck(ID))
            {
                Console.Write("Update Position: ");
                newPosition = Console.ReadLine();

                if (EmployeeAppService.UpdatedOwnAccount(ID, emp.EmployeeID, newPosition))
                {
                    EmployeeAppService.UpdateEmployee(empUpdate, newPass, newPosition);
                    Console.WriteLine($"You changed your role from Admin to: {newPosition}. Logging out...\n");
                    return false;
                }
            }


            Console.WriteLine();

            EmployeeAppService.UpdateEmployee(empUpdate, newPass, newPosition);

            Console.WriteLine("Employee Updated!\n");
        }
        else
        {
            Console.WriteLine("Employee does not exist.\n");
        }
        return true;
    }
    static bool removeEmployees()
    {
        Console.Write("Remove Employee. Input ID: ");
        int ID = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();

        if (EmployeeAppService.checkEmployee(ID))
        {

            if (EmployeeAppService.AdminCountCheck(ID))
            {
                Console.WriteLine("This account is the only admin and cant be removed.\n");
                return true;
            }

            EmployeeAppService.RemoveEmployee(ID);

            if (EmployeeAppService.RemovedOwnAccount(ID, emp.EmployeeID))
            {
                Console.WriteLine("You removed yourself. Logging out...\n");
                return false;
            }

            Console.WriteLine("Employee Removed!\n");
        }
        else
        {
            Console.WriteLine("Employee does not exist.\n");
        }
        return true;

    }

    static void addEmployees()
    {
        Console.Write("First Name: ");
        string first = Console.ReadLine();

        Console.Write("Last Name: ");
        string last = Console.ReadLine();

        Console.Write("Password: ");
        string pass = Console.ReadLine();

        Console.Write("Position: ");
        string position = Console.ReadLine();

        int empID = EmployeeAppService.AddEmployee(first, last, pass, position);
        Console.WriteLine("\nEmployee Added!");
        Console.WriteLine("Employee ID: " + empID + "\n");
    }

    static void showFiledLeaves()
    {
        var leaves = EmployeeAppService.GetLeaves();

        if (leaves.Count() == 0)
        {
            Console.WriteLine("No data yet.\n");
        }
        else
        {
            Console.WriteLine("Leave ID\tEmployee ID\tEmployee Name\t\t\tType of Leave\t\tDays\t\tDate \n");

            foreach (var leave in leaves)
            {
                Console.WriteLine($"{leave.LeaveID}\t|\t{leave.EmployeeID}\t|\t{leave.Name}\t|\t{leave.TypeOfLeave}\t|\t{leave.DaysOfLeave}\t|\t{leave.DateOfLeave}");

            }
            Console.WriteLine();
        }
    }

    static void showEmployeeList()
    {
        var Employees = EmployeeAppService.GetEmployees();

        if (Employees.Count() == 0)
        {
            Console.WriteLine("No data yet.\n");
        }
        else
        {
            Console.WriteLine("EMPLOYEES:");
            foreach (var employee in Employees)
            {
                Console.WriteLine("ID: " + employee.EmployeeID + ", Name: " + employee.FirstName + " " + employee.LastName + ", Password: " + employee.Password + ", Position: " + employee.Position);

            }
            Console.WriteLine();
        }
    }
}