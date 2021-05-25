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
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        IConnectDB DB;
        static List<JobModel> import_jobs;

        public ProjectController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddJob(string id, string number, string name, int year)
        {
            this.DB = new ConnectDB();
            SqlConnection con = DB.Connect();
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Job(Job_ID, Job_Number, Job_Name, Job_Year) " +
                                                   "VALUES(@Job_ID, @Job_Number, @Job_Name, @Job_Year)", con))
            {
                con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.Parameters.Add("@Job_ID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Job_Number", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Job_Name", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Job_Year", SqlDbType.Int);
                cmd.Parameters[0].Value = id.Replace("-",String.Empty);
                cmd.Parameters[1].Value = id;
                cmd.Parameters[2].Value = name;
                cmd.Parameters[3].Value = year;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return Json("Done");
        }

        public JsonResult Import()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "files";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            import_jobs = new List<JobModel>();
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
                    sheet = hssfwb.GetSheetAt(0);
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                    sheet = hssfwb.GetSheetAt(0);
                }

                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                IRow row;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);
                    if (row == null)
                        break;
                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                        break;
                    if (row.GetCell(0).StringCellValue == "")
                        break;

                    JobModel job = new JobModel();
                    job.job_id = row.GetCell(0).StringCellValue.Trim().Replace("-",String.Empty);
                    job.job_number = row.GetCell(1).StringCellValue.Trim();
                    job.job_name = row.GetCell(2).StringCellValue;
                    job.job_year = Convert.ToInt32(row.GetCell(3).NumericCellValue);
                    import_jobs.Add(job);
                }
            }
            return Json(import_jobs);
        }

        [HttpPost]
        public JsonResult ConfirmImport()
        {
            this.DB = new ConnectDB();
            SqlConnection con = DB.Connect();
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Job(Job_ID, Job_Number, Job_Name, Job_Year) " +
                                                   "VALUES(@Job_ID, @Job_Number, @Job_Name, @Job_Year)", con))
            {
                con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.Parameters.Add("@Job_ID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Job_Number", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Job_Name", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Job_Year", SqlDbType.Int);

                for (int i = 0; i < import_jobs.Count; i++)
                {
                    cmd.Parameters[0].Value = import_jobs[i].job_id;
                    cmd.Parameters[1].Value = import_jobs[i].job_number;
                    cmd.Parameters[2].Value = import_jobs[i].job_name;
                    cmd.Parameters[3].Value = import_jobs[i].job_year;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return Json("Done");
        }
    }
}
