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
        XmlSerializer ser = new XmlSerializer(typeof(GameList));
        FileStream myFileStream = new FileStream(_env.ContentRootPath + "\\gamelist.xml", FileMode.Open);
        return (GameList)ser.Deserialize(myFileStream);
    }
}