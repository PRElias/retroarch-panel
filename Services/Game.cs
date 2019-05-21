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
    private readonly IHostingEnvironment _env;
    public GameService(IHostingEnvironment env)
    {
        _env = env;
    }
    public GameList GetGames()
    {
        List<string> dirs = new List<string>(Directory.EnumerateDirectories(_env.ContentRootPath + "\\RETROARCH\\share\\roms"));
        GameList gl = new GameList();

        foreach (var dir in dirs)
        {
            //Pegando apenas o nome da pasta
            //Console.WriteLine($"{dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)}");
            string system = dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1);

            XmlSerializer ser = new XmlSerializer(typeof(GameList));
            FileStream myFileStream = new FileStream(dir + "\\gamelist.xml", FileMode.Open);
            var retorno = (GameList)ser.Deserialize(myFileStream);
            retorno.Games.ForEach(g => g.System = system);
            gl.Games.AddRange(retorno.Games);
        }
        
        return gl;
    }
}