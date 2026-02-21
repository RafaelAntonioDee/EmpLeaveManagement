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
        bool Continue = true;
        Console.Write("Input Employee Name: ");
        EmpName = Console.ReadLine();
        while (Continue)
        {
           

            

            Console.WriteLine($"Input Type of Leave: \n1. Maternity Leave ({MaternityAvailableLeave}) \n2. Paternity Leave ({PaternityAvailableLeave}) \n3. Sick Leave ({VacationAvailableLeave}) \n4. Vacation Leave ({SickAvailableLeave}) ");
            int LeaveType = Convert.ToInt16(Console.ReadLine());

            Console.Write("Input Days of Leave: ");
            int LeaveDays = Convert.ToInt16(Console.ReadLine());

            Console.Write("Date of Leave: ");
            DateofLeave = Console.ReadLine();

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
            string YorN = Console.ReadLine();
            if (YorN == "n")
            {
                Continue = false;
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
                    output();
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
                    output();
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
                    output();
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
                    output();
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
    }

}