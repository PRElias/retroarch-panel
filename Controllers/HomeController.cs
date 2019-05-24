using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using retroarch_panel.Models;

namespace retroarch_panel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Dados([FromServices] IGameService gameService)
        {
            var jogos = gameService.GetGames();
            return View(jogos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ExcelExport()
        {
            //Create a new ExcelPackage
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "Recalbox Panel";
                excelPackage.Workbook.Properties.Title = "Recalbox games";
                // excelPackage.Workbook.Properties.Subject = "EPPlus demo export data";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("All Games");

                //Add some text to cell A1
                worksheet.Cells["A1"].Value = "My first EPPlus spreadsheet!";
                //You could also use [line, column] notation:
                worksheet.Cells[1, 2].Value = "This is cell B1!";

                //Save your file
                FileInfo fi = new FileInfo(@"P:\Paulo Elias\RETROARCH\share\roms\File.xlsx");
                excelPackage.SaveAs(fi);
            }

            return Ok();
        }
    }
}
