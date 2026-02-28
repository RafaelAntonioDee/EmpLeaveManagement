using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

internal class EmpLeaveManagement
{
    static int MaternityAvailableLeave = 105, PaternityAvailableLeave = 7, VacationAvailableLeave = 15, SickAvailableLeave = 15;
    static List<String> TypeOfLeaves = new List<String>();
    static List<int> DaysOfLeaves = new List<int>();
    static List<String> LeaveDates = new List<String>();

    static String EmpName, DateofLeave;
    static void Main(string[] args)
    {
        bool ContinueSystem = true;
        bool ContinueFileLeave = true;
        while (ContinueSystem)
        {

            Console.WriteLine("======================== EMPLOYEE LEAVE MANAGEMENT ========================");
            Console.WriteLine("(1) File Leave \n(2) Login as Admin \n(3) Close System");
            Console.Write("Input: ");
            int Choice = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();


            if (Choice == 1)
            {
                Console.WriteLine("======================== FILE LEAVE ========================");

                Console.Write("Input Employee Name: ");
                EmpName = Console.ReadLine();
                Console.WriteLine();

                while (ContinueFileLeave)
                {

                    Console.WriteLine($"Input\t\tType of Leave\t\tAvailable Days\n(1)\t|\tMaternity Leave\t|\t{MaternityAvailableLeave} \n(2)\t|\tPaternity Leave\t|\t{PaternityAvailableLeave} \n(3)\t|\tSick Leave\t|\t{VacationAvailableLeave} \n(4)\t|\tVacation Leave\t|\t{SickAvailableLeave} ");
                    Console.Write("Input: ");
                    int LeaveType = Convert.ToInt16(Console.ReadLine());
                    Console.WriteLine();

                    Console.Write("Input Days of Leave: ");
                    int LeaveDays = Convert.ToInt16(Console.ReadLine());
                    Console.WriteLine();


                    Console.Write("Date of Leave: ");
                    DateofLeave = Console.ReadLine();
                    Console.WriteLine();


                    String type = "";
                    switch (LeaveType)
                    {
                        case 1:
                            type = "Maternity Leave";
                            break;
                        case 2:
                            type = "Paternity Leave";
                            break;
                        case 3:
                            type = "Sick Leave";
                            break;
                        case 4:
                            type = "Vacation Leave";
                            break;

                    }
                    TypeOfLeaves.Add(type);
                    DaysOfLeaves.Add(LeaveDays);
                    LeaveDates.Add(DateofLeave);


                    CalculateAvailableLeaveDays(LeaveType, LeaveDays);




                    Console.WriteLine("Would you like to account another leave? (y/n)");
                    Console.Write("Input: ");
                    string YorN = Console.ReadLine();
                    if (YorN == "n")
                    {
                        ContinueFileLeave = false;
                    }
                }

                Console.WriteLine();

            }
            else if (Choice == 2)
            {
                Console.WriteLine("======================== ADMIN DASHBOARD ========================");
                Console.WriteLine("(1) View Leave History");
                Console.Write("Input: ");
                Choice = Convert.ToInt16(Console.ReadLine());

                Console.WriteLine();

                if (Choice == 1)
                {
                    output();
                }
            }
            else
            {
                Console.WriteLine();

                Environment.Exit(0);
            }

        }
    }

    static void CalculateAvailableLeaveDays(int TypeOfLeave, int Days)
    {
        switch (TypeOfLeave)
        {
            case 1:
                if (Days <= MaternityAvailableLeave)
                {
                    MaternityAvailableLeave -= Days;

                }
                else
                {
                    Console.WriteLine($"{Days} exceeds {MaternityAvailableLeave} days of available leaves");
                }
                break;
            case 2:
                if (Days <= PaternityAvailableLeave)
                {
                    PaternityAvailableLeave -= Days;
                }
                else
                {
                    Console.WriteLine($"{Days} exceeds {PaternityAvailableLeave} days of available leaves");
                }
                break;
            case 3:
                if (Days <= SickAvailableLeave)
                {
                    SickAvailableLeave -= Days;
                }
                else
                {
                    Console.WriteLine($"{Days} exceeds {SickAvailableLeave} days of available leaves");
                }
                break;
            case 4:
                if (Days <= VacationAvailableLeave)
                {
                    VacationAvailableLeave -= Days;
                }
                else
                {
                    Console.WriteLine($"{Days} exceeds {VacationAvailableLeave} days of available leaves");

                }
                break;

        }
    }
    static void output()
    {
        for (int i = 0; i < TypeOfLeaves.Count; i++)
        {
            Console.WriteLine($"Employee Name: {EmpName}, Type of Leave: {TypeOfLeaves[i]}, Days: {DaysOfLeaves[i]}, Date: {LeaveDates[i]}");
        }
        Console.WriteLine();


    }

}