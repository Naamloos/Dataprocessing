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
        [Key]
        [Column("Position", TypeName = "INT(11)")]
        public int Position{ get; set; }

        [Column("TrackName")]
        public string TrackName{ get; set; }

        [Column("Artist")]
        public string Artist{ get; set; }

        [Column("Streams")]
        public long Streams{ get; set; }

        [Column("Url")]
        public string Url{ get; set; }

        [Key]
        [Column("Date")]
        public DateTime Date{ get; set; }

        [Key]
        [Column("Region")]
        public string Region{ get; set; }
    }
}
