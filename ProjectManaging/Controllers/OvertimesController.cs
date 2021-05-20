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
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class OvertimesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        static List<OvertimeModel> jobs;
        IConnectDB DB;

        public OvertimesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<string> GetJobID()
        {
            List<string> job_ids = new List<string>();
            this.DB = new ConnectDB();
            SqlConnection con = DB.Connect();
            con.Open();
            string str_cmd = "select Job_ID from Job";
            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string id = dr["Job_ID"].ToString().Trim();
                    job_ids.Add(id);
                }
                dr.Close();
            }
            con.Close();
            return job_ids;
        }

        [HttpGet]
        public JsonResult GetJobNumbers()
        {
            return Json(GetJobID());
        }

        public JsonResult Import()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "files";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

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

                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                IRow row;
                for (int i = 1; i < sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);
                    if (row == null)
                        break;
                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                        break;
                    if (row.Cells.All(c => c.NumericCellValue == 0))
                        break;

                    OvertimeModel ot = new OvertimeModel();
                    DateTime rt = row.GetCell(1).DateCellValue;
                    ot.job_id = "";
                    ot.employee_id = row.GetCell(2).StringCellValue;
                    ot.ot_1_5 = Convert.ToInt32(row.GetCell(3).NumericCellValue);
                    ot.ot_3 = Convert.ToInt32(row.GetCell(4).NumericCellValue);
                    ot.ot_sum = ot.ot_1_5 + ot.ot_3;
                    //ot.week = 
                    //ot.month = 
                    //ot.recording_time =
                    jobs.Add(ot);
                }
            }
            return Json(jobs);
        }
    }
}
