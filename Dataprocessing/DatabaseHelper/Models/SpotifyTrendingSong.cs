using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DatabaseHelper.Models
{
    [Table("spotify")]
    public class SpotifyTrendingSong
    {
        [XmlElement(ElementName ="position")]
        [JsonPropertyName("position")]
        [Key]
        [Column("Position", TypeName = "INT(11)")]
        public int Position{ get; set; }

        [XmlElement(ElementName = "trackname")]
        [JsonPropertyName("trackname")]
        [Column("TrackName")]
        public string TrackName{ get; set; }

        [XmlElement(ElementName = "artist")]
        [JsonPropertyName("artist")]
        [Column("Artist")]
        public string Artist{ get; set; }

        [XmlElement(ElementName = "streams")]
        [JsonPropertyName("streams")]
        [Column("Streams")]
        public long Streams{ get; set; }

        [XmlElement(ElementName = "url")]
        [JsonPropertyName("url")]
        [Column("Url")]
        public string Url{ get; set; }

        [XmlElement(ElementName = "date")]
        [JsonPropertyName("date")]
        [Key]
        [Column("Date")]
        public DateTime Date{ get; set; }

        [XmlElement(ElementName = "region")]
        [JsonPropertyName("region")]
        [Key]
        [Column("Region")]
        public string Region{ get; set; }
    }
}
