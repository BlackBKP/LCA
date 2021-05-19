using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class Job
    {
        public string job_id { get; set; }

        public string job_name { get; set; }

        public int month { get; set; }

        public int week { get; set; }

        public string week_time { get; set; }

        public int labor_cost { get; set; }

        public int ot_cost { get; set; }
        
        public int accomodate { get; set; }

        public int compensate { get; set; }

        public int cost_to_date { get; set; }

        public int number_of_labor { get; set; }
    }

    public class ImportJobController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImportJobController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ImportOT()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "files";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();

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

                sb.Append("<table class='table table-sm table-hover' style='width:100%'>");
                sb.Append("<thead><tr>");
                for (int i = 1; i < cellCount; i++)
                {
                    sb.Append("<td>" + headerRow.GetCell(i) + "</td>");
                }
                sb.Append("</tr></thead>");

                IRow row;
                sb.Append("<tbody>");
                for (int i = 1; i < sheet.LastRowNum;i++)
                {
                    row = sheet.GetRow(i);
                    if (row == null) 
                        continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) 
                        continue;
                    if (row.Cells.All(c => c.NumericCellValue == 0))
                        continue;

                    sb.Append("<tr>");
                    for (int j = 1; j < cellCount; j++)
                    {
                        if(j == 1)
                            sb.Append("<td>" + row.GetCell(j).DateCellValue + "</td>");
                        else
                            sb.Append("<td>" + row.GetCell(j).NumericCellValue + "</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
            }
            return this.Content(sb.ToString());
        }

        public ActionResult Import2()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "files";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            List<Job> jobs = new List<Job>();

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

                sb.Append("<table class='table table-sm table-hover' style='width:100%'>");
                sb.Append("<thead><tr>");
                for (int i = 5; i < cellCount; i++)
                {
                    if (i == 7)
                        sb.Append("<td>Month</td><td>Week</td><td>Week Time</td>");
                    else
                        sb.Append("<td>" + headerRow.GetCell(i) + "</td>");
                }
                sb.Append("</tr></thead>");

                IRow row;
                sb.Append("<tbody>");
                for (int i = 3; i < sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);
                    if (row == null)
                        continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                        continue;
                    if (row.Cells.All(c => c.NumericCellValue == 0))
                        continue;
                    Job job = new Job();
                    sb.Append("<tr>");
                    for (int j = 5; j < cellCount; j++)
                    {
                        string[] months = new string[] { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
                        if (j == 5)
                            job.job_id = row.GetCell(5).StringCellValue;
                        else if (j == 6)
                            job.job_name = row.GetCell(6).StringCellValue;
                        else if (j == 7)
                        {
                            string str = row.GetCell(j).StringCellValue;
                            string month = str.Split(' ')[1].ToUpper().Substring(0, 3);
                            job.week = str.Split('-')[0] == "1" ? 1 : 2;
                            job.month = (Array.IndexOf(months, month) + 1);
                        }
                        else if (j == 8)
                            job.labor_cost = Convert.ToInt32(row.GetCell(j).NumericCellValue);
                        else if (j == 9)
                            job.ot_cost = Convert.ToInt32(row.GetCell(j).NumericCellValue);
                        else if (j == 10)
                            job.accomodate = Convert.ToInt32(row.GetCell(j).NumericCellValue);
                        else if (j == 11)
                            job.compensate = Convert.ToInt32(row.GetCell(j).NumericCellValue);
                        else if (j == 12)
                            job.number_of_labor = Convert.ToInt32(row.GetCell(j).NumericCellValue);

                        if (j == 7)
                        {
                            string str = row.GetCell(j).StringCellValue;
                            int week = str.Split('-')[0] == "1" ? 1 : 2;
                            string month = str.Split(' ')[1].ToUpper().Substring(0, 3);
                            sb.Append("<td>" + Array.IndexOf(months,month) + 1 + "</td>");//month
                            sb.Append("<td>" + week + "</td>");//week
                            sb.Append("<td>" + row.GetCell(j).StringCellValue + "</td>");//Week Time
                        }  
                        else if(j==5 || j == 6)
                        {
                            sb.Append("<td>" + row.GetCell(j).StringCellValue + "</td>");
                        }
                        else
                        {
                            sb.Append("<td>" + row.GetCell(j).NumericCellValue + "</td>");
                        }
                    }
                    sb.Append("</tr>");
                    job.cost_to_date = job.labor_cost + job.ot_cost + job.accomodate + job.compensate;
                    jobs.Add(job);
                }
                sb.Append("</tbody></table>");
            }
            var vv = jobs;
            return this.Content(sb.ToString());
        }

        public ActionResult Import3()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "files";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();

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

                sb.Append("<table class='table table-sm table-hover' style='width:100%'>");
                sb.Append("<thead><tr>");
                sb.Append("<td>Staff ID</td><td>Date</td><td>Time</td>");
                sb.Append("</tr></thead>");

                IRow row;
                sb.Append("<tbody>");
                for (int i = 0; i < sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);
                    if (row == null)
                        continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                        continue;
                    if (row.Cells.All(c => c.NumericCellValue == 0))
                        continue;
                    if (row.GetCell(5).DateCellValue.ToString() == "31/12/99 00:00:00")
                        continue;

                    sb.Append("<tr>");
                    for (int j = 4; j < cellCount; j++)
                    {
                        if (j == 4)
                            sb.Append("<td>" + row.GetCell(j).StringCellValue + "</td>");
                        else if (j == 5)
                            sb.Append("<td>" + row.GetCell(j).DateCellValue + "</td>");
                        else if (j == 6)
                            sb.Append("<td>" + row.GetCell(j).StringCellValue + "</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
            }

            return this.Content(sb.ToString());
        }
    }
}
