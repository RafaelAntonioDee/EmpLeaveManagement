using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

internal class EmpLeaveManagement
{
    static int MaternityAvailableLeave = 105, PaternityAvailableLeave = 7, VacationAvailableLeave = 15, SickAvailableLeave = 15;
    static List<String> TypeOfLeaves = new List<String>();
    static List<int> DaysOfLeaves = new List<int>();
    static List<String> LeaveDates = new List<String>();
    static List<String> EmployeeNames = new List<String>();

    class Employee
    {
        public string Name;
        public int MaternityLeave = 105;
        public int PaternityLeave = 7;
        public int VacationLeave = 15;
        public int SickLeave = 15;
    }
    static List<Employee> Employees = new List<Employee>();
    
    static void Main(string[] args)
    {
        bool continueSystem = true;
        bool continueFileLeave = true;
        bool continueAsAdmin = true;
        while (continueSystem)
        {

            int Choice = checkMenuChoice();


            if (Choice == 1)
            {
                Console.WriteLine("======================== FILE LEAVE ========================");

                Console.Write("Input Employee Name: ");
                String EmpName = Console.ReadLine();
                Console.WriteLine();
                Employee emp = new Employee();
                emp.Name = EmpName;
                Employees.Add(emp);

                while (continueFileLeave)
                {
                    String LeaveType = checkLeaveType(EmpName);
                    int LeaveDays = checkDaysOfLeave();

                    Console.Write("Date of Leave: ");
                    String DateofLeave = Console.ReadLine();
                    Console.WriteLine();

                    CalculateAvailableLeaveDays(EmpName, LeaveType, LeaveDays);

                    

                    EmployeeNames.Add(EmpName);
                    TypeOfLeaves.Add(LeaveType);
                    DaysOfLeaves.Add(LeaveDays);
                    LeaveDates.Add(DateofLeave);

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
            else if (Choice == 2)
            {
                while (continueAsAdmin)
                {
                    Console.WriteLine("======================== ADMIN DASHBOARD ========================");
                    Console.WriteLine("[1] View Leave History\n[2] Logout");
                    Console.Write("Input: ");
                    Choice = Convert.ToInt16(Console.ReadLine());

                    Console.WriteLine();

                    if (Choice == 1)
                    {
                        output();
                    }
                    else if(Choice == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Option does not exist, choose again.\n");
                    }
                }
            }
            else
            {
                Console.WriteLine();

                Environment.Exit(0);
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
    static String checkLeaveType(String EmployeeName)
    {
        while (true)
        {
            foreach (Employee emp in Employees)
            {
                if(emp.Name == EmployeeName)
                {
                   Console.WriteLine($"Input\t\tType of Leave\t\tAvailable Days\n" +
                   $"[1]\t|\tMaternity Leave\t|\t{emp.MaternityLeave} \n" +
                   $"[2]\t|\tPaternity Leave\t|\t{emp.PaternityLeave} \n" +
                   $"[3]\t|\tSick Leave\t|\t{emp.VacationLeave} \n" +
                   $"[4]\t|\tVacation Leave\t|\t{emp.SickLeave} ");
                }
                break;
               
            }
            Console.Write("Input: ");
            int LeaveType = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();
            switch (LeaveType)
            {
                case 1:
                    return "Maternity Leave";
                    break;
                case 2:
                    return "Paternity Leave";
                    break;
                case 3:
                    return "Sick Leave";
                    break;
                case 4:
                    return "Vacation Leave";
                    break;
                default:
                    Console.WriteLine("Option does not exist, choose again.\n");
                    break;
            }
        }
    }
    static int checkDaysOfLeave()
    {
        while (true)
        {
            Console.Write("Input Days of Leave: ");
            int DaysofLeave = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();

            if (DaysofLeave <= 0)
            {
                Console.WriteLine($"Please input a valid amount.");
                Console.WriteLine();
            }
            else if(DaysofLeave > MaternityAvailableLeave)
            {
                Console.WriteLine($"{DaysofLeave} exceeds {MaternityAvailableLeave} days of available leaves.");
                Console.WriteLine();
            }
            else {
                return DaysofLeave;
            }
        }
    }
    static void CalculateAvailableLeaveDays(String EmployeeName, String TypeOfLeave, int Days)
    {
        foreach (Employee emp in Employees)
        {
            if (emp.Name == EmployeeName)
            {
                switch (TypeOfLeave)
                {
                    case "Maternity Leave":

                        emp.MaternityLeave -= Days;
                        break;
                    case "Paternity Leave":
                        emp.PaternityLeave -= Days;
                        break;
                    case "Sick Leave":
                        emp.SickLeave -= Days;
                        break;
                    case "Vacation Leave":
                        emp.VacationLeave -= Days;
                        break;
                }
                break;
            }
        }
    }
    static void output()
    {
        if (EmployeeNames.Count() == 0)
        {
            Console.WriteLine("No data yet.\n");
        }
        else
        {
            for (int i = 0; i < TypeOfLeaves.Count; i++)
            {
                Console.WriteLine($"Employee Name: {EmployeeNames[i]}, Type of Leave: {TypeOfLeaves[i]}, Days: {DaysOfLeaves[i]}, Date: {LeaveDates[i]}");
            }
            Console.WriteLine();
        }
    }

}