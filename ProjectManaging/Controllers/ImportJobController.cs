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

        public ActionResult Import()
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
    }
}
