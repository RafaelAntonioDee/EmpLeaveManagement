using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EmpLeaveManagementAppModel;
using Microsoft.Data.SqlClient;

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
            var existingAcc = GetAdmins();
            var existingLeaves = GetLeaves();
            var existingEmps = GetEmployees();

            if (existingAcc.Count <= 0)
            {
                AdminAccount adminAcc = new AdminAccount { AccountID = Guid.NewGuid(), Username = "dee", Password = "dee123" };
                AddAdmin(adminAcc);
            }
            if (existingEmps.Count <= 0)
            {
                Employee emp1 = new Employee { EmployeeID = Guid.NewGuid(), Name = "Rafael Antonio Dee" };
                Employee emp2 = new Employee { EmployeeID = Guid.NewGuid(), Name = "Indaleen Quinsayas" };
                AddEmployee(emp1);
                AddEmployee(emp2);
            }
        }

        // ----------------------------------------------------ADD FUNCTIONS----------------------------------------------------
        public void AddLeave(FiledLeave Leave, Employee emp)
        {
            sqlConnection.Open();

            var updateStatement = $"UPDATE tblEmployees SET MaternityLeave = @MaternityLeave, PaternityLeave = @PaternityLeave, VacationLeave = @VacationLeave, SickLeave = @SickLeave WHERE EmployeeID = @EmployeeID";

            SqlCommand updateCommand = new SqlCommand(updateStatement, sqlConnection);

            updateCommand.Parameters.AddWithValue("@MaternityLeave", emp.MaternityLeave);
            updateCommand.Parameters.AddWithValue("@PaternityLeave", emp.PaternityLeave);
            updateCommand.Parameters.AddWithValue("@VacationLeave", emp.VacationLeave);
            updateCommand.Parameters.AddWithValue("@SickLeave", emp.SickLeave);
            updateCommand.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
            updateCommand.ExecuteNonQuery();

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
            var insertStatement = "INSERT INTO tblEmployees VALUES (@EmployeeID, @Name, @MaternityLeave, @PaternityLeave, @VacationLeave, @SickLeave)";

            SqlCommand cmd = new SqlCommand(insertStatement, sqlConnection);

            cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@MaternityLeave", employee.MaternityLeave);
            cmd.Parameters.AddWithValue("@PaternityLeave", employee.PaternityLeave);
            cmd.Parameters.AddWithValue("@VacationLeave", employee.VacationLeave);
            cmd.Parameters.AddWithValue("@SickLeave", employee.SickLeave);
            sqlConnection.Open();

            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }
        public void AddAdmin(AdminAccount admin)
        {
            var insertStatement = "INSERT INTO tblAdminAccounts VALUES (@AccountID, @Username, @Password)";

            SqlCommand cmd = new SqlCommand(insertStatement, sqlConnection);

            cmd.Parameters.AddWithValue("@AccountID", admin.AccountID);
            cmd.Parameters.AddWithValue("@Username", admin.Username);
            cmd.Parameters.AddWithValue("@Password", admin.Password);
            sqlConnection.Open();

            cmd.ExecuteNonQuery();

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
        }
        public void RemoveAdmin(AdminAccount admin)
        {
            var deleteStatement = "DELETE FROM tblAdminAccounts WHERE AccountID = @AccountID";

            SqlCommand cmd = new SqlCommand(deleteStatement, sqlConnection);

            cmd.Parameters.AddWithValue("@AccountID", admin.AccountID);
            sqlConnection.Open();

            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }


        // ----------------------------------------------------CHECK EXISTENCE FUNCTIONS----------------------------------------------------
        public bool EmployeeExists(string empName)
        {
            var selectStatement = "SELECT * FROM tblEmployees WHERE Name = @Name";
            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);
            cmd.Parameters.AddWithValue("@Name", empName);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            var emp = new Employee();

            while (reader.Read())
            {
                emp.EmployeeID = Guid.Parse(reader["EmployeeID"].ToString());
                emp.Name = reader["Name"].ToString();
                emp.MaternityLeave = int.Parse(reader["MaternityLeave"].ToString());
                emp.PaternityLeave = int.Parse(reader["PaternityLeave"].ToString());
                emp.VacationLeave = int.Parse(reader["VacationLeave"].ToString());
                emp.SickLeave = int.Parse(reader["SickLeave"].ToString());
            }

            sqlConnection.Close();
            return emp.Name != null;
        }
        public bool AdminExists(string username)
        {
            var selectStatement = "SELECT * FROM tblAdminAccounts WHERE Username = @Username";
            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);
            cmd.Parameters.AddWithValue("@Username", username);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            var admin = new AdminAccount();

            while (reader.Read())
            {
                admin.AccountID = Guid.Parse(reader["AccountID"].ToString());
                admin.Username = reader["Username"].ToString();
                admin.Password = reader["Password"].ToString();
            }

            sqlConnection.Close();
            return admin.Username != null;
        }


        // ----------------------------------------------------GET FUNCTIONS----------------------------------------------------
        public Employee? GetEmployeeByName(string name)
        {
            var selectStatement = "SELECT * FROM tblEmployees WHERE Name = @Name";
            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);
            cmd.Parameters.AddWithValue("@Name", name);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            var emp = new Employee();

            while (reader.Read())
            {
                emp.EmployeeID = Guid.Parse(reader["EmployeeID"].ToString());
                emp.Name = reader["Name"].ToString();
                emp.MaternityLeave = int.Parse(reader["MaternityLeave"].ToString());
                emp.PaternityLeave = int.Parse(reader["PaternityLeave"].ToString());
                emp.VacationLeave = int.Parse(reader["VacationLeave"].ToString());
                emp.SickLeave = int.Parse(reader["SickLeave"].ToString());
            }

            sqlConnection.Close();
            return emp;
        }
        public AdminAccount? GetAdminByUser(string user)
        {
            var selectStatement = "SELECT * FROM tblAdminAccounts WHERE Username = @Username";
            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);
            cmd.Parameters.AddWithValue("@UserName", user);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            var account = new AdminAccount();

            while (reader.Read())
            {
                account.AccountID = Guid.Parse(reader["AccountID"].ToString());
                account.Username = reader["Username"].ToString();
                account.Password = reader["Password"].ToString();
            }

            sqlConnection.Close();
            return account;
        }
        public AdminAccount? AccountGetByUsername(string username)
        {
            var selectStatement = "SELECT * FROM tblAdminAccounts WHERE Username = @Username";
            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);
            cmd.Parameters.AddWithValue("@UserName", username);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            var account = new AdminAccount();

            while (reader.Read())
            {
                account.AccountID = Guid.Parse(reader["AccountID"].ToString());
                account.Username = reader["Username"].ToString();
                account.Password = reader["Password"].ToString();
            }

            sqlConnection.Close();
            return account;
        }
        public Employee? GetById(Guid id)
        {
            var selectStatement = "SELECT * FROM tblEmployees WHERE EmployeeID = @EmployeeID";
            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);
            cmd.Parameters.AddWithValue("@EmployeeID", id);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            var emp = new Employee();

            while (reader.Read())
            {
                emp.EmployeeID = Guid.Parse(reader["EmployeeID"].ToString());
                emp.Name = reader["Name"].ToString();
                emp.MaternityLeave = int.Parse(reader["MaternityLeave"].ToString());
                emp.PaternityLeave = int.Parse(reader["PaternityLeave"].ToString());
                emp.VacationLeave = int.Parse(reader["VacationLeave"].ToString());
                emp.SickLeave = int.Parse(reader["SickLeave"].ToString());
            }

            sqlConnection.Close();
            return emp;
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
                leave.LeaveID = Guid.Parse(reader["LeaveID"].ToString());
                leave.EmployeeID = Guid.Parse(reader["EmployeeID"].ToString());
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
                emp.EmployeeID = Guid.Parse(reader["EmployeeID"].ToString());
                emp.Name = reader["Name"].ToString();
                emp.MaternityLeave = int.Parse(reader["MaternityLeave"].ToString());
                emp.PaternityLeave = int.Parse(reader["PaternityLeave"].ToString());
                emp.VacationLeave = int.Parse(reader["VacationLeave"].ToString());
                emp.SickLeave = int.Parse(reader["SickLeave"].ToString());

                employees.Add(emp);
            }

            sqlConnection.Close();
            return employees;
        }
        public List<AdminAccount> GetAdmins()
        {
            string selectStatement = "SELECT * FROM tblAdminAccounts";

            SqlCommand cmd = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            var accounts = new List<AdminAccount>();

            while (reader.Read())
            {
                AdminAccount account = new AdminAccount();
                account.AccountID = Guid.Parse(reader["AccountID"].ToString());
                account.Username = reader["Username"].ToString();
                account.Password = reader["Password"].ToString();

                accounts.Add(account);
            }

            sqlConnection.Close();
            return accounts;
        }
    }
}
