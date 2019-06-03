using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace retroarch_panel.Models
{
    [XmlRoot(ElementName = "game")]
    public class Game
    {
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "desc")]
        public string Description {get; set;}
        [XmlElement(ElementName = "image")]
        public string Image { get; set; }
        [XmlElement(ElementName = "rating")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "releasedate")]
        public string ReleaseDate { get; set; }
        [XmlElement(ElementName = "developer")]
        public string Developer { get; set; }
        [XmlElement(ElementName = "publisher")]
        public string Publisher { get; set; }
        [XmlElement(ElementName = "genre")]
        public string Genre { get; set; }
        [XmlElement(ElementName = "players")]
        public string Players { get; set; }
        [XmlElement(ElementName = "playcount")]
        public string Playcount { get; set; }
        [XmlElement(ElementName = "lastplayed")]
        public string Lastplayed { get; set; }
        public string System { get; set; }
        public int panelGameId { get; set; }
        public string PanelImage { get; set; }
    }

    [XmlRoot(ElementName = "gameList")]
    public class GameList
    {
        [XmlElement(ElementName = "game")]
        public List<Game> Games { get; set; }

        public Guid Id { get; set; }

        public GameList()
        {
            Games = new List<Game>();
        }
    }
}
