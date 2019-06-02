using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            if (HttpContext.Session.GetString("games") == null)
            {
                gameList = gameService.GetGames();
                HttpContext.Session.SetString("games", JsonConvert.SerializeObject(gameService.GetGames()));
            }
            else
            {
                gameList = JsonConvert.DeserializeObject<GameList>(HttpContext.Session.GetString("games"));
            }

            return View(gameList);
        }

        public IActionResult Detalhes(int id)
        {
            gameList = JsonConvert.DeserializeObject<GameList>(HttpContext.Session.GetString("games"));
            var gameDetalhe = gameList.Games.Find(g => g.panelGameId == id);
            return View(gameDetalhe);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void ExcelExport([FromServices] IGameService gameService)
        {
            //Create a new ExcelPackage
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                List<string[]> headerRow = new List<string[]>()
                {
                    new string[] { "Rom", "Name", "Description", "Image", "Rating", "Release Date", "Developer", "Publisher", "Genre", "Players", "Playcount", "Lastplayed", "System" }
                };

                // Determine the header range (e.g. A1:E1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "Recalbox Panel";
                excelPackage.Workbook.Properties.Title = "Recalbox games";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("All Games");

                if (HttpContext.Session.GetString("games") == null)
                {
                    gameList = gameService.GetGames();
                    HttpContext.Session.SetString("games", JsonConvert.SerializeObject(gameService.GetGames()));
                }
                else
                {
                    gameList = JsonConvert.DeserializeObject<GameList>(HttpContext.Session.GetString("games"));
                }

                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[headerRange].Style.Font.Bold = true;
                worksheet.Cells[2, 1].LoadFromCollection(gameList.Games);

                //Save your file
                // FileInfo fi = new FileInfo(@"C:\Users\ppusp\Downloads\File.xlsx");
                // excelPackage.SaveAs(fi);

                this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.Headers.Add(
                          "content-disposition",
                          string.Format("attachment;  filename={0}", "AllGames.xlsx"));
                this.Response.Body.WriteAsync(excelPackage.GetAsByteArray());
            }

            // return Ok();
        }

        public void StopEmulationStation()
        {
            Ssh ssh = new Ssh();
            var teste = ssh.ExecuteCommand("killall emulationstation");
            this.Response.Redirect("Home/Dados");
        }

        // public void StartEmulationStation()
        // {
        //     Ssh ssh = new Ssh();
        //     var teste = ssh.ExecuteCommand("/etc/init.d/S31emulationstation start");
        //     this.Response.Redirect("Home/Dados");
        // }
    }
}
