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

        public List<SpentPerWeekModel> GetSpentCostPerWeeks()
        {
            List<SpentPerWeekModel> spws = new List<SpentPerWeekModel>();
            SqlConnection con = DB.Connect();
            con.Open();

            string str_cmd = "select Labor_Costs.Job_ID, " +
                                    "SUM((cast(Labor_Cost as int) + cast(OT_Labor_Cost as int) + cast(Accommodation_Cost as int) + cast(Compensation_Cost as int))) OVER(PARTITION BY Labor_Costs.job_ID ORDER BY Labor_Costs.job_ID ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) as Acc_Cost, " +
                                    "week, " +
                                    "Labor_Costs.Month, " +
                                    "Labor_Costs.Year, " +
                                    "(s1.Estimated_Budget * 1.0) as Budget100, " +
                                    "(s1.Estimated_Budget * 0.8) as Budget80, " +
                                    "(s1.Estimated_Budget * 0.7) as Budget70, " +
                                    "(s1.Estimated_Budget * 0.5) as Budget50, " +
                                    "((cast(s2.Job_Progress as int) + lag(s2.Job_Progress,1) over (partition by s2.Job_ID order by s2.Job_ID))/2.0) as Progress, " +
                                    "(cast(Labor_Cost as int) + cast(OT_Labor_Cost as int) + cast(Accommodation_Cost as int) + cast(Compensation_Cost as int)) as spent_cost " +
                                    "from Labor_Costs left join (select job_ID,Estimated_Budget from job) as s1 ON s1.job_ID = Labor_Costs.job_ID " +
                                    "left join (select Job_ID, Job_Progress, Month, Year from Progress) as s2 ON s2.Job_ID = Labor_Costs.job_ID and s2.Year = Labor_Costs.Year and s2.Month = Labor_Costs.Month " +
                                    "order by Labor_Costs.job_ID, Labor_Costs.Year, Labor_Costs.Month, Labor_Costs.week";

            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    SpentPerWeekModel spw = new SpentPerWeekModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        week = dr["week"] != DBNull.Value ? Convert.ToInt32(dr["week"]) : 0,
                        month = dr["Month"] != DBNull.Value ? Convert.ToInt32(dr["Month"]) : 0,
                        year = dr["Year"] != DBNull.Value ? Convert.ToInt32(dr["Year"]) : 0,
                        budget100 = dr["Budget100"] != DBNull.Value ? Convert.ToInt32(dr["budget100"]) : 0,
                        budget80 = dr["Budget80"] != DBNull.Value ? Convert.ToInt32(dr["Budget80"]) : 0,
                        budget70 = dr["Budget70"] != DBNull.Value ? Convert.ToInt32(dr["Budget70"]) : 0,
                        budget50 = dr["Budget50"] != DBNull.Value ? Convert.ToInt32(dr["Budget50"]) : 0,
                        progress = dr["Progress"] != DBNull.Value ? Convert.ToInt32(dr["Progress"]) : 0,
                        spent_cost = dr["spent_cost"] != DBNull.Value ? Convert.ToInt32(dr["spent_cost"]) : 0,
                        acc_cost = dr["Acc_Cost"] != DBNull.Value ? Convert.ToInt32(dr["Acc_Cost"]) : 0,
                    };
                    spws.Add(spw);
                }
                dr.Close();
            }

            con.Close();
            return spws;
        }

        public List<SpentPerWeekModel> GetSpentPerWeeksByJob(string job_id)
        {
            List<SpentPerWeekModel> spws = new List<SpentPerWeekModel>();
            SqlConnection con = DB.Connect();
            con.Open();

            string str_cmd = "select Labor_Costs.Job_ID, " +
                                    "SUM((cast(Labor_Cost as int) + cast(OT_Labor_Cost as int) + cast(Accommodation_Cost as int) + cast(Compensation_Cost as int))) OVER(PARTITION BY Labor_Costs.job_ID ORDER BY Labor_Costs.job_ID ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) as Acc_Cost, " +
                                    "week, " +
                                    "Labor_Costs.Month, " +
                                    "Labor_Costs.Year, " +
                                    "(s1.Estimated_Budget * 1.0) as Budget100, " +
                                    "(s1.Estimated_Budget * 0.8) as Budget80, " +
                                    "(s1.Estimated_Budget * 0.7) as Budget70, " +
                                    "(s1.Estimated_Budget * 0.5) as Budget50, " +
                                    "((cast(s2.Job_Progress as int) + lag(s2.Job_Progress,1) over (partition by s2.Job_ID order by s2.Job_ID))/2.0) as Progress, " +
                                    "(cast(Labor_Cost as int) + cast(OT_Labor_Cost as int) + cast(Accommodation_Cost as int) + cast(Compensation_Cost as int)) as spent_cost " +
                                    "from Labor_Costs left join (select job_ID,Estimated_Budget from job) as s1 ON s1.job_ID = Labor_Costs.job_ID " +
                                    "left join (select Job_ID, Job_Progress, Month, Year from Progress) as s2 ON s2.Job_ID = Labor_Costs.job_ID and s2.Year = Labor_Costs.Year and s2.Month = Labor_Costs.Month " +
                                    "where Labor_Costs.job_ID = '" + job_id + "' " +
                                    "order by Labor_Costs.job_ID, Labor_Costs.Year, Labor_Costs.Month, Labor_Costs.week";

            SqlCommand cmd = new SqlCommand(str_cmd, con);

            return spws;
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
                        employee_id = dr["Employee_ID"] != DBNull.Value ? dr["Employee_ID"].ToString() : "",
                        employee_name = dr["Employee_Name"] != DBNull.Value ? dr["Employee_Name"].ToString() : "",
                        employee_surname = dr["Employee_Surname"] != DBNull.Value ? dr["Employee_Surname"].ToString() : "",
                        employee_position = dr["Employee_Position"] != DBNull.Value ? dr["Employee_Position"].ToString() : ""
                    };
                    emps.Add(emp);
                }
                dr.Close();
            }

            con.Close();
            return emps;
        }

        public List<HourModel> GetHours()
        {
            List<HourModel> hours = new List<HourModel>();
            SqlConnection con = DB.Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * FROM Hour", con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    HourModel hour = new HourModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        employee_id = dr["Employee_ID"] != DBNull.Value ? dr["Employee_ID"].ToString() : "",
                        working_day = dr["Working_Day"] != DBNull.Value ? Convert.ToDateTime(dr["Working_Day"]) : DateTime.MinValue,
                        week = dr["Week"] != DBNull.Value ? Convert.ToInt32(dr["Week"]) : 0,
                        month = dr["Month"] != DBNull.Value ? Convert.ToInt32(dr["Month"]) : 0,
                        hours = dr["Hours"] != DBNull.Value ? Convert.ToInt32(dr["Hours"]) : 0
                    };
                    hours.Add(hour);
                }
                dr.Close();
            }

            con.Close();
            return hours;
        }

        public List<JobModel> GetJobs()
        {
            List<JobModel> jobs = new List<JobModel>();
            SqlConnection con = DB.Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * FROM Job", con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    JobModel job = new JobModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                        estimated_budget = dr["Estimated_Budget"] != DBNull.Value ? Convert.ToInt32(dr["Estimated_Budget"]) : 0,
                        cost_to_date = dr["Cost_to_date"] != DBNull.Value ? Convert.ToInt32(dr["Cost_to_date"]) : 0,
                        remaining_cost = dr["Remaining_Cost"] != DBNull.Value ? Convert.ToInt32(dr["Remaining_Cost"]) : 0,
                        cost_usage = dr["Cost_Usage"] != DBNull.Value ? Convert.ToInt32(dr["Cost_Usage"]) : 0,
                        work_completion = dr["Work_Completion"] != DBNull.Value ? Convert.ToInt32(dr["Work_Completion"]) : 0,
                        total_normal_man_hour = dr["Total_Normal_Man_Hour"] != DBNull.Value ? Convert.ToInt32(dr["Total_Normal_Man_Hour"]) : 0,
                        no_of_labor = dr["No_Of_Labor"] != DBNull.Value ? Convert.ToInt32(dr["No_Of_Labor"]) : 0,
                        avg_cost_per_hour = dr["Avg_Cost_Per_Hour"] != DBNull.Value ? Convert.ToInt32(dr["Avg_Cost_Per_Hour"]) : 0,
                        job_year = dr["Job_Year"] != DBNull.Value ? Convert.ToInt32(dr["Job_Year"]) : 0,
                        lastest_update = dr["Latest_Update"] != DBNull.Value ? Convert.ToDateTime(dr["Latest_Update"]) : DateTime.MinValue,
                    };
                    jobs.Add(job);
                }
                dr.Close();
            }

            con.Close();
            return jobs;
        }

        public List<LaborCostModel> GetLaborCosts()
        {
            List<LaborCostModel> lcs = new List<LaborCostModel>();
            SqlConnection con = DB.Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * FROM Labor_Costs", con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LaborCostModel lc = new LaborCostModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        week = dr["Week"] != DBNull.Value ? Convert.ToInt32(dr["Week"]) : 0,
                        month = dr["Month"] != DBNull.Value ? Convert.ToInt32(dr["Month"]) : 0,
                        week_time = dr["Week_time"] != DBNull.Value ? dr["Week_time"].ToString() : "",
                        labor_cost = (dr["Labor_Cost"] != DBNull.Value && dr["Labor_Cost"].ToString() != "") ? Convert.ToInt32(dr["Labor_Cost"]) : 0 ,
                        ot_labor_cost = (dr["OT_Labor_Cost"] != DBNull.Value && dr["OT_Labor_Cost"].ToString() != "") ? Convert.ToInt32(dr["OT_Labor_Cost"]) : 0,
                        accommodation_cost = (dr["Accommodation_Cost"] != DBNull.Value && dr["Accommodation_Cost"].ToString() != "") ? Convert.ToInt32(dr["Accommodation_Cost"]) : 0,
                        compensation_cost = (dr["Compensation_Cost"] != DBNull.Value && dr["Compensation_Cost"].ToString() != "") ? Convert.ToInt32(dr["Compensation_Cost"]) : 0,
                        no_of_labor_week = (dr["No_Of_Labor_Week"] != DBNull.Value && dr["No_Of_Labor_Week"].ToString() != "") ? Convert.ToInt32(dr["No_Of_Labor_Week"]) : 0
                    };
                    lcs.Add(lc);
                }
                dr.Close();
            }

            con.Close();
            return lcs;
        }

        public List<OvertimeModel> GetOvertimes()
        {
            List<OvertimeModel> ots = new List<OvertimeModel>();
            SqlConnection con = DB.Connect();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * FROM OT", con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    OvertimeModel ot = new OvertimeModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        employee_id =  dr["Employee_ID"] != DBNull.Value ? dr["Employee_ID"].ToString() : "",
                        ot_1_5 = dr["OT_1_5"] != DBNull.Value ? Convert.ToInt32(dr["OT_1_5"]) : 0,
                        ot_3 =  dr["OT_3"] != DBNull.Value ? Convert.ToInt32(dr["OT_3"]) : 0,
                        ot_sum =  dr["OT_Sum"] != DBNull.Value ? Convert.ToInt32(dr["OT_Sum"]) : 0,
                        week = dr["Week"] != DBNull.Value ? Convert.ToInt32(dr["Week"]) : 0,
                        month =  dr["Month"] != DBNull.Value ? Convert.ToInt32(dr["Month"]) : 0,
                        recording_time = dr["Recording_Time"] != DBNull.Value ? Convert.ToDateTime(dr["Recording_Time"]) : DateTime.MinValue
                    };
                    ots.Add(ot);
                }
                dr.Close();
            }

            con.Close();
            return ots;
        }
    }
}
