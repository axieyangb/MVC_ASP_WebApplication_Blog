using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
    [Table("ImageView")]
    public class ImageViewModel
    {
        [Key]
        [Column("ImageID")]
        public long ImageId { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.ImageUrl)]
        [Column("Url")]
        public string Url { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [Column("UpdateDate")]
        public DateTime UpdateDate { get; set; }
        [Column("UserID")]
        public long UserId { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [Column("DeleteTime")]
        public DateTime? DeleteTime { get; set; }
        [Column("isPublish")]
        public byte IsPublish { get; set; }
        [Column("isBlock")]
        public byte IsBlock { get; set; }
        [Column("ContentType")]
        public string ContentType { get; set; }
        [Column("FileName")]
        public string FileName { get; set; }
    }
}