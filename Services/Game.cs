using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using retroarch_panel.Models;

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
        List<string> dirs = new List<string>(Directory.EnumerateDirectories(RecalboxShare));
        GameList gl = new GameList();
        int panelGameId = 0;

        foreach (var dir in dirs)
        {
            string system = dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1);

            XmlSerializer ser = new XmlSerializer(typeof(GameList));

            if (File.Exists(dir + "\\gamelist.xml"))
            {
                using (FileStream myFileStream = new FileStream(dir + "\\gamelist.xml", FileMode.Open))
                {
                    var retorno = (GameList)ser.Deserialize(myFileStream);

                    foreach (var jogo in retorno.Games)
                    {
                        jogo.System = system;
                        jogo.panelGameId = panelGameId++;
                        if (jogo.Image != null & jogo.Image != String.Empty)
                        {
                            jogo.Image = "~/images/" + system + jogo.Image.Substring(1, jogo.Image.Length -1).Replace("//", @"\");
                        }
                    }
                    gl.Games.AddRange(retorno.Games);
                }
            }
        }
        return gl;
    }
}