using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EmpLeaveManagementAppModel;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace EmpLeaveManagementDataService
{
    public class LeaveManagementDBData : ILeaveManagementDataService
    {
        private string connectionString = "Data Source =localhost\\SQLEXPRESS; Initial Catalog = EmpLeaveManagement; Integrated Security = True; TrustServerCertificate = True;";
        private SqlConnection sqlConnection;

        public LeaveManagementDBData()
        {
            sqlConnection = new SqlConnection(connectionString);
            AddSeeds();
        }

        private void AddSeeds()
        {
            var existingLeaves = GetLeaves();
            var existingEmps = GetEmployees();

            if (existingEmps.Count <= 0)
            {
                Employee employee1 = new Employee { EmployeeID = 100000, FirstName = "Rafael Antonio", LastName = "Dee", Password = "dee123", Position = "Admin" };
                Employee employee2 = new Employee { EmployeeID = 100001, FirstName = "Indaleen", LastName = "Quinsayas", Password = "123", Position = "Supervisor" };
                Employee employee3 = new Employee { EmployeeID = 100002, FirstName = "John", LastName = "Doe", Password = "123", Position = "Sales" };
                AddEmployee(employee1);
                AddEmployee(employee2);
                AddEmployee(employee3);
            }
        }

        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public void AddLeave(FiledLeave Leave, Employee emp)
        {
            sqlConnection.Open();
            var insertStatement = "INSERT INTO tblFiledLeaves VALUES (@LeaveID, @EmployeeID, @Name, @TypeOfLeave, @DaysOfLeave, @DateOfLeave)";

            SqlCommand cmd = new SqlCommand(insertStatement, sqlConnection);

            cmd.Parameters.AddWithValue("@LeaveID", Leave.LeaveID);
            cmd.Parameters.AddWithValue("@EmployeeID", Leave.EmployeeID);
            cmd.Parameters.AddWithValue("@Name", Leave.Name);
            cmd.Parameters.AddWithValue("@TypeOfLeave", Leave.TypeOfLeave);
            cmd.Parameters.AddWithValue("@DaysOfLeave", Leave.DaysOfLeave);
            cmd.Parameters.AddWithValue("@DateOfLeave", Leave.DateOfLeave);

            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }
        public void AddEmployee(Employee employee)
        {
            sqlConnection.Open();

            var insertEmployeeStatement = "INSERT INTO tblEmployees VALUES (@EmployeeID, @FirstName, @LastName, @Password, @Position)";

            SqlCommand cmdEmployee = new SqlCommand(insertEmployeeStatement, sqlConnection);

            cmdEmployee.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
            cmdEmployee.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmdEmployee.Parameters.AddWithValue("@LastName", employee.LastName);
            cmdEmployee.Parameters.AddWithValue("@Password", employee.Password);
            cmdEmployee.Parameters.AddWithValue("@Position", employee.Position);

            cmdEmployee.ExecuteNonQuery();
            sqlConnection.Close();

            EmployeeLeaveData empLeaveData = new EmployeeLeaveData();

            sqlConnection.Open();

            var insertLeaveDataStatement = "INSERT INTO tblEmployeeLeaveData VALUES (@EmployeeID, @MaternityLeave, @PaternityLeave, @VacationLeave, @SickLeave)";

            SqlCommand cmdLeaveData = new SqlCommand(insertLeaveDataStatement, sqlConnection);

            cmdLeaveData.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
            cmdLeaveData.Parameters.AddWithValue("@MaternityLeave", empLeaveData.MaternityLeave);
            cmdLeaveData.Parameters.AddWithValue("@PaternityLeave", empLeaveData.PaternityLeave);
            cmdLeaveData.Parameters.AddWithValue("@VacationLeave", empLeaveData.VacationLeave);
            cmdLeaveData.Parameters.AddWithValue("@SickLeave", empLeaveData.SickLeave);

            cmdLeaveData.ExecuteNonQuery();

            sqlConnection.Close();
        }

        // ----------------------------------------------------UPDATE FUNCTIONS----------------------------------------------------
        public void UpdateEmployee(Employee employee, string newPass, string newPosition)
        {
            sqlConnection.Open();

            var updateStatement = $"UPDATE tblEmployees SET Password = @Password, Position = @Position WHERE EmployeeID = @EmployeeID";

            SqlCommand updateCommand = new SqlCommand(updateStatement, sqlConnection);

            updateCommand.Parameters.AddWithValue("@Password", newPass);
            updateCommand.Parameters.AddWithValue("@Position", newPosition);
            updateCommand.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);

            updateCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }

        // ----------------------------------------------------REMOVE FUNCTIONS----------------------------------------------------
        public void RemoveEmployee(Employee employee)
        {
            var deleteStatement = "DELETE FROM tblEmployees WHERE EmployeeID = @EmployeeID";

            SqlCommand cmd = new SqlCommand(deleteStatement, sqlConnection);

            cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
            sqlConnection.Open();

            cmd.ExecuteNonQuery();

            sqlConnection.Close();

            deleteStatement = "DELETE FROM tblEmployeeLeaveData WHERE EmployeeID = @EmployeeID";

             cmd = new SqlCommand(deleteStatement, sqlConnection);

            cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
            sqlConnection.Open();

            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }

        // ----------------------------------------------------CHECK EXISTENCE FUNCTIONS----------------------------------------------------
        public bool EmployeeExists(int id)
        {
            Employee emp = GetEmployee(id);
            return emp != null;
        }


        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------
        public Employee? GetEmployee(int id)
        {
            var selectStatement = "SELECT * FROM tblEmployees WHERE EmployeeID = @EmployeeID";
            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);
            cmd.Parameters.AddWithValue("@EmployeeID", id);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                sqlConnection.Close();
                return null;
            }

            var emp = new Employee();

            while (reader.Read())
            {
                emp.EmployeeID = int.Parse(reader["EmployeeID"].ToString());
                emp.FirstName = reader["FirstName"].ToString();
                emp.LastName = reader["LastName"].ToString();
                emp.Password = reader["Password"].ToString();
                emp.Position = reader["Position"].ToString();
            }


            sqlConnection.Close();
            return emp;
        }
        public EmployeeLeaveData? GetEmployeeLeaveData(int empid)
        {
            var selectStatement = "SELECT * FROM tblEmployeeLeaveData WHERE EmployeeID = @EmployeeID";
            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);
            cmd.Parameters.AddWithValue("@EmployeeID", empid);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            var empLeaveData = new EmployeeLeaveData();

            while (reader.Read())
            {
                empLeaveData.EmployeeID = int.Parse(reader["EmployeeID"].ToString());
                empLeaveData.MaternityLeave = int.Parse(reader["MaternityLeave"].ToString());
                empLeaveData.PaternityLeave = int.Parse(reader["PaternityLeave"].ToString());
                empLeaveData.VacationLeave = int.Parse(reader["VacationLeave"].ToString());
                empLeaveData.SickLeave = int.Parse(reader["SickLeave"].ToString());
            }

            if (empLeaveData.EmployeeID <= 0)
            {
                sqlConnection.Close();
                return null;
            }
            sqlConnection.Close();
            return empLeaveData;
        }

        public int GetNewLeaveID()
        {
            int newId;

            var selectStatement = @"SELECT ISNULL(MAX(LeaveID), 99999) + 1 FROM tblFiledLeaves";

            using (SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection))
            {
                sqlConnection.Open();
                newId = Convert.ToInt32(cmd.ExecuteScalar());
                sqlConnection.Close();
            }

            return newId;
        }
       

        public int GetNewEmployeeID()
        {
            int newId;

            var selectStatement = @"SELECT ISNULL(MAX(EmployeeID), 99999) + 1 FROM tblEmployees";

            using (SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection))
            {
                sqlConnection.Open();
                newId = Convert.ToInt32(cmd.ExecuteScalar());
                sqlConnection.Close();
            }

            return newId;
        }

        public void CalculateAvailableDays(int empID, string TypeOfLeave, int Days)
        {
            TypeOfLeave = TypeOfLeave.Replace(" ", "");
            EmployeeLeaveData empLeaveData = GetEmployeeLeaveData(empID);
            sqlConnection.Open();

            var updateStatement = $"UPDATE tblEmployeeLeaveData SET {TypeOfLeave} = @newLeaveDays WHERE EmployeeID = @EmployeeID";

            SqlCommand updateCommand = new SqlCommand(updateStatement, sqlConnection);

            switch (TypeOfLeave)
            {
                case "MaternityLeave":
                    updateCommand.Parameters.AddWithValue("@newLeaveDays", (empLeaveData.MaternityLeave -= Days));
                    updateCommand.Parameters.AddWithValue("@EmployeeID", empID);

                    break;
                case "PaternityLeave":
                    updateCommand.Parameters.AddWithValue("@newLeaveDays", (empLeaveData.PaternityLeave -= Days));
                    updateCommand.Parameters.AddWithValue("@EmployeeID", empID);

                    break;
                case "SickLeave":
                    updateCommand.Parameters.AddWithValue("@newLeaveDays", (empLeaveData.SickLeave -= Days));
                    updateCommand.Parameters.AddWithValue("@EmployeeID", empID);

                    break;
                case "VacationLeave":
                    updateCommand.Parameters.AddWithValue("@newLeaveDays", (empLeaveData.VacationLeave -= Days));
                    updateCommand.Parameters.AddWithValue("@EmployeeID", empID);

                    break;
            }

            updateCommand.ExecuteNonQuery();

            sqlConnection.Close();

        }


        // ----------------------------------------------------GET LISTS FUNCTIONS----------------------------------------------------
        public List<FiledLeave> GetLeaves()
        {
            string selectStatement = "SELECT * FROM tblFiledLeaves";

            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            var filedLeaves = new List<FiledLeave>();

            while (reader.Read())
            {
                FiledLeave leave = new FiledLeave();
                leave.LeaveID = int.Parse(reader["LeaveID"].ToString());
                leave.EmployeeID = int.Parse(reader["EmployeeID"].ToString());
                leave.Name = reader["Name"].ToString();
                leave.TypeOfLeave = reader["TypeOfLeave"].ToString();
                leave.DaysOfLeave = int.Parse(reader["DaysOfLeave"].ToString());
                leave.DateOfLeave = reader["DateOfLeave"].ToString();

                filedLeaves.Add(leave);
            }

            sqlConnection.Close();
            return filedLeaves;
        }
        public List<Employee> GetEmployees()
        {
            string selectStatement = "SELECT * FROM tblEmployees";

            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            var employees = new List<Employee>();

            while (reader.Read())
            {
                Employee emp = new Employee();
                emp.EmployeeID = int.Parse(reader["EmployeeID"].ToString());
                emp.FirstName = reader["FirstName"].ToString();
                emp.LastName = reader["LastName"].ToString();
                emp.Password = reader["Password"].ToString();
                emp.Position = reader["Position"].ToString();

                employees.Add(emp);
            }

            sqlConnection.Close();
            return employees;
        }


    }
}
