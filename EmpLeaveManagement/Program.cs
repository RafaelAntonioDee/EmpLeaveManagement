using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using EmpLeaveManagementAppModel;
using EmpLeaveManagementAppService;

internal class EmpLeaveManagement
{
    static AppService EmployeeAppService = new AppService();

    // ----------------------------------------------------MAIN----------------------------------------------------
    static void Main(string[] args)
    {
        bool continueSystem = true;
        while (continueSystem)
        {
            int Choice = checkMenuChoice();
            if (Choice == 1)
            {
                EmployeeFileLeave();

            }
            else if (Choice == 2)
            {
                AdminLogin();
            }
            else
            {
                Console.WriteLine("System Closed.");

                Environment.Exit(0);
            }
        }
    }


    // ----------------------------------------------------MENU FUNCTIONS----------------------------------------------------
    static int checkMenuChoice()
    {
        while (true)
        {
            Console.WriteLine("======================== EMPLOYEE LEAVE MANAGEMENT ========================");
            Console.WriteLine("[1] File Leave \n[2] Login as Admin \n[3] Close System");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();

            if ((Choice <= 0) || Choice > 3)
            {
                Console.WriteLine("Option does not exist, choose again.\n");
            }
            else
            {
                return Choice;
            }
        }
    }


    // ----------------------------------------------------EMPLOYEE FUNCTIONS----------------------------------------------------
    static void EmployeeFileLeave()
    {
        bool continueFileLeave = true;
        Console.WriteLine("======================== FILE LEAVE ========================");

        while (true)
        {
            Console.Write("Input Employee Name: ");
            String EmpName = Console.ReadLine();
            Console.WriteLine();

            Employee Emp;

            if (EmployeeAppService.checkEmployee(EmpName))
            {
                Emp = EmployeeAppService.GetEmployee(EmpName);
                while (continueFileLeave)
                {
                    String LeaveType = setLeaveType(Emp);
                    int LeaveDays = setDaysOfLeave(LeaveType, Emp);
                    String LeaveDate = setDateOfLeave();

                    EmployeeAppService.CalculateAvailableLeaveDays(Emp.Name, LeaveType, LeaveDays);

                    FiledLeave newLeave = new FiledLeave { EmployeeID = Emp.EmployeeID, Name = Emp.Name, TypeOfLeaves = LeaveType, DaysOfLeaves = LeaveDays, DateOfLeave = LeaveDate };
                    EmployeeAppService.RecordLeave(newLeave);

                    Console.WriteLine("Would you like to account another leave? (y/n)");
                    Console.Write("Input: ");
                    string YorN = Console.ReadLine();
                    if (YorN == "n")
                    {
                        continueFileLeave = false;
                    }
                }
                continueFileLeave = true;
                Console.WriteLine();
                break;
            }
            else
            {
                Console.WriteLine("Employee does not exist.\n");
            }
        }
    }
    static String setLeaveType(Employee Emp)
    {

        while (true)
        {

            Console.WriteLine($"Input\t\tType of Leave\t\tAvailable Days\n" +
            $"[1]\t|\tMaternity Leave\t|\t{Emp.MaternityLeave} \n" +
            $"[2]\t|\tPaternity Leave\t|\t{Emp.PaternityLeave} \n" +
            $"[3]\t|\tSick Leave\t|\t{Emp.SickLeave} \n" +
            $"[4]\t|\tVacation Leave\t|\t{Emp.VacationLeave} ");

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
            if (EmployeeAppService.checkDaysOfLeaveAvailable(LeaveType, Emp) > 0)
            {
                return LeaveType;
            }
            else
            {
                Console.WriteLine($"There is no available days left for {LeaveType}, Choose again.\n");
            }
        }
    }
    static int setDaysOfLeave(string LeaveType, Employee emp)
    {
        while (true)
        {
            int DaysofLeave = 0;

            if (LeaveType == "Maternity Leave") {
                DaysofLeave = 105;
            }
            else
            {
                Console.Write("Input Days of Leave: ");
                DaysofLeave = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine();
            }

            int LeaveTypeAvailable = EmployeeAppService.checkDaysOfLeaveAvailable(LeaveType, emp);
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
    static void AdminLogin()
    {
        Console.WriteLine("======================== ADMIN LOGIN ========================");

        for (int i = 0; i < 3; i++)
        {
            Console.Write("Enter username: ");
            string usernameInput = Console.ReadLine();
            Console.Write("Enter password: ");
            string passwordInput = Console.ReadLine();
            Console.WriteLine();

            bool isMatched = EmployeeAppService.Authenticate(usernameInput, passwordInput);
            if (isMatched)
            {
                AdminDashboard();
                break;
            }
            else
            {
                Console.WriteLine("Incorrect Credentials.");
            }
        }
    }
    static void AdminDashboard()
    {
        bool continueAsAdmin = true;
        while (continueAsAdmin)
        {
            Console.WriteLine("======================== ADMIN DASHBOARD ========================");
            Console.WriteLine("[1] View Leave History\n[2] Manage Employees\n[3] Manage Admins\n[4] Logout");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine();

            if (Choice == 1)
            {
                showFiledLeaves();
            }
            else if (Choice == 2)
            {
                manageEmployees();
            }
            else if (Choice == 3)
            {
                manageAdmins();
            }
            else if (Choice == 4)
            {
                break;
            }
            else
            {
                Console.WriteLine("Option does not exist, choose again.\n");
            }
        }
    }
    static void manageAdmins()
    {
        while (true)
        {
            Console.WriteLine("======================== MANAGE ADMINS ========================");
            Console.WriteLine("[1] View Admin List\n[2] Add Admin\n[3] Remove Admin\n[4] Exit");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine();

            if (Choice == 1)
            {
                showAdminList();
            }
            else if (Choice == 2)
            {
                addAdmins();
            }
            else if (Choice == 3)
            {
                removeAdmins();
            }
            else if (Choice == 4)
            {
                break;
            }
            else
            {
                Console.WriteLine("Option does not exist, choose again.\n");
            }
        }
    }
    static void removeAdmins()
    {
        if (EmployeeAppService.getAdmins().Count > 1)
        {
            Console.Write("Remove Admin: ");
            string user = Console.ReadLine();
            Console.WriteLine();

            if (EmployeeAppService.checkAdmin(user))
            {
                EmployeeAppService.RemoveAdmin(user);
            }
            else
            {
                Console.WriteLine("Admin does not exist.\n");
            }
        }
        else
        {
            Console.WriteLine("Need to keep atleast 1 admin in the system");
        }
    }
    static void addAdmins()
    {
        Console.Write("Add admin username: ");
        string username = Console.ReadLine();
        Console.Write("Add admin password: ");
        string password = Console.ReadLine();

        if (EmployeeAppService.checkEmployee(username))
        {
            Console.WriteLine("Admin already exists.\n");
        }
        else
        {
            EmployeeAppService.AddAdmin(username, password);
        }
    }
    static void manageEmployees()
    {
        while (true)
        {
            Console.WriteLine("======================== MANAGE EMPLOYEES ========================");
            Console.WriteLine("[1] View Employees List\n[2] Add Employees\n[3] Remove Employees\n[4] Exit");
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
                removeEmployees();
            }
            else if (Choice == 4)
            {
                break;
            }
            else
            {
                Console.WriteLine("Option does not exist, choose again.\n");
            }
        }
    }
    static void removeEmployees()
    {
        Console.Write("Remove Employee Name: ");
        string empName = Console.ReadLine();
        Console.WriteLine();

        if (EmployeeAppService.checkEmployee(empName))
        {
            EmployeeAppService.RemoveEmployee(empName);
        }
        else
        {
            Console.WriteLine("Employee does not exist.\n");
        }
    }
    static void addEmployees()
    {
        Console.Write("Add Employee Name: ");
        string empName = Console.ReadLine();

        if (EmployeeAppService.checkEmployee(empName))
        {
            Console.WriteLine("Employee already exists.\n");
        }
        else
        {
            EmployeeAppService.AddEmployee(empName);
        }
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
            foreach (var leave in leaves)
            {
                Console.WriteLine($"Employee Name: {leave.Name}, Type of Leave: {leave.TypeOfLeaves}, Days: {leave.DaysOfLeaves}, Date: {leave.DateOfLeave}");

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
                Console.WriteLine(employee.Name);

            }
            Console.WriteLine();
        }
    }
    static void showAdminList()
    {
        var Admins = EmployeeAppService.getAdmins();

        if (Admins.Count() == 0)
        {
            Console.WriteLine("No data yet.\n");
        }
        else
        {
            Console.WriteLine("ADMINS:");
            foreach (var admin in Admins)
            {
                Console.WriteLine($"Username: {admin.Username}, Password: {admin.Password}");

            }
            Console.WriteLine();
        }
    }

}