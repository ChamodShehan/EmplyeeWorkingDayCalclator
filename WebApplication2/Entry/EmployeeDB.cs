using System.Data;
using System.Data.SqlClient;
using WebApplication2.Models;

namespace WebApplication2.Entry
{
    public class EmployeeDB
    {
 
        private readonly string _connectionString;
         
        public EmployeeDB(string connectionString)
        {
            _connectionString = connectionString;
        }
         
        public List<Employee> ListAll()
        {
            List<Employee> lst = new List<Employee>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand com = new SqlCommand("SelectEmployee", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Employee
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        Email = rdr["Email"].ToString(),
                        JobPosition = rdr["JobPosition"].ToString(),
                    });
                }
                return lst;
            }
        } 

        public int Add(Employee emp)
        {
            int i;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand com = new SqlCommand("InsertUpdateEmployee", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", emp.Id);
                com.Parameters.AddWithValue("@Name", emp.Name);
                com.Parameters.AddWithValue("@Email", emp.Email);
                com.Parameters.AddWithValue("@JobPosition", emp.JobPosition);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
            }
            return i;
        }

   
        public int Update(Employee emp)
        {
            int result;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand com = new SqlCommand("InsertUpdateEmployee", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", emp.Id);
                com.Parameters.AddWithValue("@Name", emp.Name);
                com.Parameters.AddWithValue("@Email", emp.Email);
                com.Parameters.AddWithValue("@JobPosition", emp.JobPosition);
                com.Parameters.AddWithValue("@Action", "Update");
                result = com.ExecuteNonQuery();
            }
            return result;
        }
         
        public int Delete(int ID)
        {
            int result;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeleteEmployee", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", ID);
                result = com.ExecuteNonQuery();
            }
            return result;
        }
 
        public Employee GetById(int ID)
        {
            Employee employee = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetEmployeeById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                JobPosition = reader.GetString(reader.GetOrdinal("JobPosition"))
                            };
                        }
                    }
                }
            }

            return employee;
        }
         
        public List<DateTime> GetPublicHolidays()
        {
            var holidays = new List<DateTime>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Date FROM PublicHolidays", con))  
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            holidays.Add(reader.GetDateTime(reader.GetOrdinal("Date")));
                        }
                    }
                }
            }

            return holidays;
        }
    }
}
