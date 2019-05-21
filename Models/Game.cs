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
        [XmlElement(ElementName = "playcount")]
        public string Playcount { get; set; }
        [XmlElement(ElementName = "lastplayed")]
        public string Lastplayed { get; set; }
    }

    [XmlRoot(ElementName = "gameList")]
    public class GameList
    {
        [XmlElement(ElementName = "game")]
        public List<Game> Game { get; set; }
    }
}
