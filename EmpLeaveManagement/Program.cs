using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using EmpLeaveManagementAppModel;
using EmpLeaveManagementAppService;

internal class EmpLeaveManagement
{
    static AppService EmployeeAppService = new AppService();

    static void Main(string[] args)
    {
        bool continueSystem = true;
        while (continueSystem)
        {

            if (checkMenuChoice() == 1)
            {
                EmployeeFileLeave();

            }
            else if (checkMenuChoice() == 2)
            {
                AdminDashboard();
            }
            else
            {
                Console.WriteLine("System Closed.");

                Environment.Exit(0);
            }
        }
    }
    static void EmployeeFileLeave()
    {
        bool continueFileLeave = true;
        Console.WriteLine("======================== FILE LEAVE ========================");

        Console.Write("Input Employee Name: ");
        String EmpName = Console.ReadLine();
        Console.WriteLine();

        Employee Emp;

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
    }
    static void AdminDashboard()
    {
        bool continueAsAdmin = true;
        while (continueAsAdmin)
        {
            Console.WriteLine("======================== ADMIN DASHBOARD ========================");
            Console.WriteLine("[1] View Leave History\n[2] View Employee List\n[3] Logout");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine();

            if (Choice == 1)
            {
                showFiledLeaves();
            }
            else if (Choice == 2)
            {
                showEmployeeList();
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
            Console.Write("Input Days of Leave: ");
            int DaysofLeave = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();

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
            Console.WriteLine("======================== EMPLOYEES ========================");
            foreach (var employee in Employees)
            {
                Console.WriteLine(employee.Name);

            }
            Console.WriteLine();
        }
    }
}