using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using retroarch_panel.Models;

namespace retroarch_panel.Services
{
    public interface IGameService
    {
        GameList GetGames();
    }
    public class GameService : IGameService
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly string RecalboxShare;
        public GameService(IHostingEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
            RecalboxShare = _config.GetValue<string>("RecalboxShare");
        }
        public GameList GetGames()
        {
            // Open database (or create if not exits)
            using (var db = new LiteDatabase(@"MyGames.db"))
            {
                // Get customer collection
                var games = db.GetCollection<GameList>("gamelist");

                if (games.Count() == 0)
                {
                    Console.WriteLine("Base de dados não encontrada");
                    List<string> dirs = new List<string>(Directory.EnumerateDirectories(RecalboxShare));
                    GameList gl = new GameList();
                    int panelGameId = 0;

                    foreach (var dir in dirs)
                    {
                        string system = dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                        var localbkp = "localBackup\\" + system;

                        System.IO.Directory.CreateDirectory(localbkp);

                        XmlSerializer ser = new XmlSerializer(typeof(GameList));

                        if (File.Exists(dir + "\\gamelist.xml"))
                        {
                            System.IO.File.Copy(dir + "\\gamelist.xml", localbkp + "\\gamelist.xml", true);
                            var dImages = dir + "\\downloaded_images";

                            if (System.IO.Directory.Exists(dImages))
                            {
                                Console.WriteLine("Copiando imagens do " + system);
                                var count = 0;
                                System.IO.Directory.CreateDirectory(localbkp + "\\downloaded_images");

                                string[] files = System.IO.Directory.GetFiles(dImages);

                                // Copy the files and overwrite destination files if they already exist.
                                foreach (string s in files)
                                {
                                    // Use static Path methods to extract only the file name from the path.
                                    var fileName = System.IO.Path.GetFileName(s);
                                    var destFile = System.IO.Path.Combine(localbkp + "\\downloaded_images\\" + fileName);
                                    if (!new System.IO.FileInfo(destFile).Exists)
                                    {
                                        System.IO.File.Copy(s, destFile, false);
                                        count++;
                                        Console.WriteLine("Arquivo copiado: " + destFile);
                                    }
                                }
                                Console.WriteLine("Total de imagens copiadas: " + count);
                            }

                            using (FileStream myFileStream = new FileStream(localbkp + "\\gamelist.xml", System.IO.FileMode.Open))
                            {
                                var retorno = (GameList)ser.Deserialize(myFileStream);

                                foreach (var jogo in retorno.Games)
                                {
                                    jogo.System = system;
                                    jogo.panelGameId = panelGameId++;
                                    if (jogo.Image != null & jogo.Image != String.Empty)
                                    {
                                        jogo.PanelImage = "~/images/" + system + jogo.Image.Substring(1, jogo.Image.Length - 1).Replace("//", @"\");
                                    }
                                }
                                gl.Games.AddRange(retorno.Games);
                            }
                        }
                    }

                    games.Insert(gl);
                }

                IEnumerable<GameList> returnedGames = games.FindAll();
                return returnedGames.First();
            }
        }
    }
}