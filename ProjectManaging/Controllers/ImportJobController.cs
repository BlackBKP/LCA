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
            string folderName = "file";
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
                    HSSFFormulaEvaluator formula = new HSSFFormulaEvaluator(hssfwb);
                    formula.EvaluateAll();
                    sheet = hssfwb.GetSheetAt(1); //get first sheet from workbook
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                    XSSFFormulaEvaluator formula = new XSSFFormulaEvaluator(hssfwb);
                    formula.EvaluateAll();
                    sheet = hssfwb.GetSheetAt(1); //get first sheet from workbook   
                }

                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = 5;/*headerRow.LastCellNum;*/

                sb.Append("<table class='table table-sm table-hover' style='width:100%'>");
                for(int i = 0; i < sheet.LastRowNum;i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) 
                        continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) 
                        continue;
                    
                    if (row.Cells.All(r => r.StringCellValue == null))
                        continue;

                    if(i == 0)
                    {
                        sb.Append("<thead><tr>");
                        for(int j = 0; j < cellCount; j++)
                        {
                            if(row.GetCell(j) != null)
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }
                        sb.Append("</tr></thead>");
                    }
                    else
                    {
                        sb.Append("<tbody><tr>");
                        for (int j = 0; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                            {
                                if (row.GetCell(j).CellType == CellType.String)
                                    sb.Append("<td>" + row.GetCell(j).StringCellValue + "</td>");
                                else if (row.GetCell(j).CellType == CellType.Numeric)
                                    sb.Append("<td>" + row.GetCell(j).NumericCellValue.ToString() + "</td>");
                                else
                                    sb.Append("<td></td>");
                            }
                        }
                        sb.Append("</tr></tbody>");
                    }
                }
                sb.Append("</table>");
            }
            return this.Content(sb.ToString());
        }
    }
}
