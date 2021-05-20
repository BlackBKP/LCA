using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ProjectManaging.Interfaces;
using ProjectManaging.Models;
using ProjectManaging.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ProjectManaging.Controllers
{
    public class ImportSpentController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        static List<JobSpentModel> jobs;
        IConnectDB DB;

        public ImportSpentController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult ImportSpent()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "files";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            jobs = new List<JobSpentModel>();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                var stream = new FileStream(fullPath, FileMode.Create);
                file.CopyTo(stream);
                stream.Position = 0;
                if (sFileExtension == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats
                    sheet = hssfwb.GetSheetAt(1);
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                    sheet = hssfwb.GetSheetAt(1);
                }
                IRow headerRow = sheet.GetRow(2);
                int cellCount = headerRow.LastCellNum;
                IRow row;
                for (int i = 3; i < sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);
                    if (row.GetCell(5).CellType == CellType.Blank)
                        break;
                    if (row == null)
                        break;
                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                        break;
                    if (row.Cells.All(c => c.NumericCellValue == 0))
                        break;
                    JobSpentModel job = new JobSpentModel();
                    string ss = row.GetCell(5).StringCellValue;
                    job.job_id = ss.Split('-')[0].Trim() + ss.Split('-')[1].Trim();
                    job.job_name = row.GetCell(6).StringCellValue;
                    string[] months = new string[] { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
                    string str = row.GetCell(7).StringCellValue;
                    string month = str.Split(' ')[1].ToUpper().Substring(0, 3);
                    job.week = str.Split('-')[0] == "1" ? 1 : 2;
                    job.month = Array.IndexOf(months, month) + 1;
                    job.year = Convert.ToInt32("20" + str.Substring(str.Length - 2));
                    job.week_time = str;
                    job.labor_cost = Convert.ToInt32(row.GetCell(8).NumericCellValue);
                    job.ot_cost = Convert.ToInt32(row.GetCell(9).NumericCellValue);
                    job.accomodate = Convert.ToInt32(row.GetCell(10).NumericCellValue);
                    job.compensate = Convert.ToInt32(row.GetCell(11).NumericCellValue);
                    job.number_of_labor = Convert.ToInt32(row.GetCell(12).NumericCellValue);
                    jobs.Add(job);
                }
            }
            return Json(jobs);
        }

        [HttpPost]
        public JsonResult ConfirmUpload()
        {

            this.DB = new ConnectDB();
            SqlConnection con = DB.Connect();
            con.Open();
            List<JobSpentModel> Job_ID = new List<JobSpentModel>();
            string Job_cmd = "select Job_ID from Job";
            SqlCommand cmd_Job = new SqlCommand(Job_cmd, con);
            SqlDataReader dr_Job = cmd_Job.ExecuteReader();
            if (dr_Job.HasRows)
            {
                while (dr_Job.Read())
                {
                    JobSpentModel j = new JobSpentModel()
                    {
                        job_id = dr_Job["Job_ID"].ToString().Trim()
                    };
                    Job_ID.Add(j);
                }
                dr_Job.Close();
            }
            con.Close();

            List<string> v1 = Job_ID.Select(s => s.job_id).ToList();
            List<string> v2 = jobs.Select(s => s.job_id).Distinct().ToList();
            //  var diff_Job = jobs.Where(w => !Job_ID.Any(a => a.job_id == w.job_id)).ToList();
            var diff_Job = v2.Except(v1).ToList();
            if (diff_Job.Count <= 0)
            {
                string str_cmd = "select Job_ID, " +
                                        "Week, " +
                                        "Month, " +
                                        "Year, " +
                                        "Week_time, " +
                                        "isnull(NULLIF(Labor_Cost,''),0)as Labor_Cost, " +
                                        "isnull(NULLIF(OT_Labor_Cost,''),0) as OT_Labor_Cost , " +
                                        "isnull(NULLIF(Accommodation_Cost,''),0) as Accommodation_Cost, " +
                                        "isnull(NULLIF(Compensation_Cost,''),0) as Compensation_Cost, " +
                                        "isnull(NULLIF(No_Of_Labor_Week,''),0) as No_Of_Labor_Week " +
                                        "from Labor_Costs";

                con.Open();
                SqlCommand cmd = new SqlCommand(str_cmd, con);
                SqlDataReader dr = cmd.ExecuteReader();

                List<JobSpentModel> uploaded_jobs = new List<JobSpentModel>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        JobSpentModel job = new JobSpentModel()
                        {
                            job_id = dr["Job_ID"].ToString(),
                            week = Convert.ToInt32(dr["Week"]),
                            month = Convert.ToInt32(dr["Month"]),
                            year = Convert.ToInt32(dr["Year"]),
                            week_time = Convert.ToString(dr["Week_Time"]),
                            labor_cost = Convert.ToInt32(dr["Labor_Cost"]),
                            ot_cost = Convert.ToInt32(dr["OT_Labor_Cost"]),
                            accomodate = Convert.ToInt32(dr["Accommodation_Cost"]),
                            compensate = Convert.ToInt32(dr["Compensation_Cost"]),
                            number_of_labor = Convert.ToInt32(dr["No_Of_Labor_Week"])
                        };
                        uploaded_jobs.Add(job);
                    }
                    dr.Close();
                }
                con.Close();
                var dif = jobs.Where(w => !uploaded_jobs.Any(y => y.job_id == w.job_id && y.week == w.week && y.month == w.month && y.year == w.year)).ToList();

                using (SqlCommand cmd3 = new SqlCommand("INSERT INTO Labor_Costs(" +
                                                                "Job_ID, " +
                                                                "Week, " +
                                                                "Month, " +
                                                                "Year, " +
                                                                "Week_time, " +
                                                                "Labor_Cost, " +
                                                                "OT_Labor_Cost, " +
                                                                "Accommodation_Cost, " +
                                                                "Compensation_Cost, " +
                                                                "No_Of_Labor_Week) " +
                                                     "VALUES(@Job_ID," +
                                                            "@Week, " +
                                                            "@Month, " +
                                                            "@Year, " +
                                                            "@Week_time, " +
                                                            "@Labor_Cost, " +
                                                            "@OT_Labor_Cost, " +
                                                            "@Accommodation_Cost, " +
                                                            "@Compensation_Cost, " +
                                                            "@No_Of_Labor_Week)", con))
                {
                    con.Open();
                    cmd3.CommandType = CommandType.Text;
                    cmd3.Connection = con;
                    cmd3.Parameters.Add("@Job_ID", SqlDbType.NVarChar);
                    cmd3.Parameters.Add("@Week", SqlDbType.Int);
                    cmd3.Parameters.Add("@Month", SqlDbType.Int);
                    cmd3.Parameters.Add("@Year", SqlDbType.Int);
                    cmd3.Parameters.Add("@Week_time", SqlDbType.NVarChar);
                    cmd3.Parameters.Add("@Labor_Cost", SqlDbType.NVarChar);
                    cmd3.Parameters.Add("@OT_Labor_Cost", SqlDbType.NVarChar);
                    cmd3.Parameters.Add("@Accommodation_Cost", SqlDbType.NVarChar);
                    cmd3.Parameters.Add("@Compensation_Cost", SqlDbType.NVarChar);
                    cmd3.Parameters.Add("@No_Of_Labor_Week", SqlDbType.NVarChar);

                    for (int i = 0; i < dif.Count; i++)
                    {
                        cmd3.Parameters[0].Value = dif[i].job_id;
                        cmd3.Parameters[1].Value = dif[i].week;
                        cmd3.Parameters[2].Value = dif[i].month;
                        cmd3.Parameters[3].Value = dif[i].year;
                        cmd3.Parameters[4].Value = dif[i].week_time;
                        cmd3.Parameters[5].Value = dif[i].labor_cost;
                        cmd3.Parameters[6].Value = dif[i].ot_cost;
                        cmd3.Parameters[7].Value = dif[i].accomodate;
                        cmd3.Parameters[8].Value = dif[i].compensate;
                        cmd3.Parameters[9].Value = dif[i].number_of_labor;
                        cmd3.ExecuteNonQuery();
                    }
                }
                con.Close();
                return Json("Done");
            }
            else
            {
                return Json(diff_Job);
            }
        }
    }
}
