using ProjectManaging.Interfaces;
using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Services
{
    public class HomeService : IHome
    {
        IConnectDB DB;
        public HomeService()
        {
             this.DB = new ConnectDB();
        }

        public List<EmployeeModel> GetEmployees()
        {
            List<EmployeeModel> emps = new List<EmployeeModel>();
            SqlConnection con = DB.Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * FROM Employee",con);
            SqlDataReader dr = cmd.ExecuteReader();
            
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    EmployeeModel emp = new EmployeeModel()
                    {
                        emp_id = dr["Employee_ID"].ToString(),
                        emp_name = dr["Employee_Name"].ToString(),
                        emp_lname = dr["Employee_Surname"].ToString(),
                        emp_pos = dr["Employee_Position"].ToString()
                    };
                    emps.Add(emp);
                }
                dr.Close();
            }
            con.Close();
            return emps;
        }
    }
}
