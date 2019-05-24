﻿using System;
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
        private GameList gameList;
        public IActionResult Index() => View();
        public IActionResult Dados([FromServices] IGameService gameService)
        {
            gameList = gameService.GetGames();
            return View(gameList);
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

        public IActionResult ExcelExport([FromServices] IGameService gameService)
        {
            //Create a new ExcelPackage
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                List<string[]> headerRow = new List<string[]>()
                {
                    new string[] { "Rom", "Name", "Playcount", "Lastplayed", "Image", "System" }
                };

                // Determine the header range (e.g. A1:E1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "Recalbox Panel";
                excelPackage.Workbook.Properties.Title = "Recalbox games";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("All Games");

                gameList = gameService.GetGames();

                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[headerRange].Style.Font.Bold = true;
                worksheet.Cells[2,1].LoadFromCollection(gameList.Games);

                //Save your file
                FileInfo fi = new FileInfo(@"C:\Users\ppusp\Downloads\File.xlsx");
                excelPackage.SaveAs(fi);
            }

            return Ok();
        }
    }
}
