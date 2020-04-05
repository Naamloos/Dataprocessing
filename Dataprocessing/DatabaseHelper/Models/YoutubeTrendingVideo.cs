using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DatabaseHelper.Models
{
    [Table("youtube")]
    public class YoutubeTrendingVideo
    {
        [XmlElement(ElementName = "videoid")]
        [JsonPropertyName("videoid")]
        [Column("VideoId")]
        public string VideoId{ get; set; }

        [XmlElement(ElementName = "trendingdate")]
        [JsonPropertyName("trendingdate")]
        [Column("TrendingDate")]
        public DateTime TrendingDate{ get; set; }

        [XmlElement(ElementName = "title")]
        [JsonPropertyName("title")]
        [Column("Title")]
        public string Title{ get; set; }

        [XmlElement(ElementName = "channeltitle")]
        [JsonPropertyName("channeltitle")]
        [Column("ChannelTitle")]
        public string ChannelTitle{ get; set; }

        [XmlElement(ElementName = "categoryid")]
        [JsonPropertyName("categoryid")]
        [Column("CategoryId")]
        public int CategoryId{ get; set; }

        [XmlElement(ElementName = "publishtime")]
        [JsonPropertyName("publishtime")]
        [Column("PublishTime")]
        public DateTime PublishTime{ get; set; }

        [XmlElement(ElementName = "tags")]
        [JsonPropertyName("tags")]
        [Column("Tags")]
        public string Tags{ get; set; }

        [XmlElement(ElementName = "views")]
        [JsonPropertyName("views")]
        [Column("Views")]
        public int Views{ get; set; }

        [XmlElement(ElementName = "likes")]
        [JsonPropertyName("likes")]
        [Column("Likes")]
        public int Likes{ get; set; }

        [XmlElement(ElementName = "dislikes")]
        [JsonPropertyName("dislikes")]
        [Column("Dislikes")]
        public int Dislikes{ get; set; }

        [XmlElement(ElementName = "commentcount")]
        [JsonPropertyName("commentcount")]
        [Column("CommentCount")]
        public int CommentCount{ get; set; }

        [XmlElement(ElementName = "thumbnaillink")]
        [JsonPropertyName("thumbnaillink")]
        [Column("ThumbnailLink")]
        public string ThumbnailLink{ get; set; }

        [XmlElement(ElementName = "commentsdisabled")]
        [JsonPropertyName("commentsdisabled")]
        [Column("CommentsDisabled", TypeName = "TINYINT(1)")]
        public bool CommentsDisabled{ get; set; }

        [XmlElement(ElementName = "ratingsdisabled")]
        [JsonPropertyName("ratingsdisabled")]
        [Column("RatingsDisabled", TypeName = "TINYINT(1)")]
        public bool RatingsDisabled{ get; set; }

        [XmlElement(ElementName = "videoerrororremoved")]
        [JsonPropertyName("videoerrororremoved")]
        [Column("VideoErrorOrRemoved", TypeName = "TINYINT(1)")]
        public bool VideoErrorOrRemoved{ get; set; }

        [XmlElement(ElementName = "description")]
        [JsonPropertyName("description")]
        [Column("Description")]
        public string Description{ get; set; }

        [XmlElement(ElementName = "countrycode")]
        [JsonPropertyName("countrycode")]
        [Column("CountryCode")]
        public string CountryCode{ get; set; }
    }
}
