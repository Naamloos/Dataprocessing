using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DatabaseHelper.Models
{
    [Table("youtube")]
    public class YoutubeTrendingVideo
    {
        [Column("VideoId")]
        public string VideoId{ get; set; }
        [Column("TrendingDate")]
        public DateTime TrendingDate{ get; set; }
        [Column("Title")]
        public string Title{ get; set; }
        [Column("ChannelTitle")]
        public string ChannelTitle{ get; set; }
        [Column("CategoryId")]
        public int CategoryId{ get; set; }
        [Column("PublishTime")]
        public DateTime PublishTime{ get; set; }
        [Column("Tags")]
        public string Tags{ get; set; }
        [Column("Views")]
        public int Views{ get; set; }
        [Column("Likes")]
        public int Likes{ get; set; }
        [Column("Dislikes")]
        public int Dislikes{ get; set; }
        [Column("CommentCount")]
        public int CommentCount{ get; set; }
        [Column("ThumbnailLink")]
        public string ThumbnailLink{ get; set; }
        [Column("CommentsDisabled")]
        public bool CommentsDisabled{ get; set; }
        [Column("RatingsDisabled")]
        public bool RatingsDisabled{ get; set; }
        [Column("VideoErrorOrRemoved")]
        public bool VideoErrorOrRemoved{ get; set; }
        [Column("Description")]
        public string Description{ get; set; }
        [Column("CountryCode")]
        public string CountryCode{ get; set; }
    }
}
