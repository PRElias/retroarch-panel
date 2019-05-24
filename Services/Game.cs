using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;
using retroarch_panel.Models;

public interface IGameService
{
    GameList GetGames();
}
public class GameService : IGameService
{
    private const string RetroarchShare = @"R:\roms";
    private readonly IHostingEnvironment _env;
    public GameService(IHostingEnvironment env)
    {
        _env = env;
    }
    public GameList GetGames()
    {
        List<string> dirs = new List<string>(Directory.EnumerateDirectories(RetroarchShare));
        GameList gl = new GameList();

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
                        if (jogo.Image != null & jogo.Image != String.Empty)
                        {
                            // jogo.Image = dir + jogo.Image.Substring(1, jogo.Image.Length -1).Replace("//", @"\");
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