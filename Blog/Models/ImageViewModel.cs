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
        public long ImageId { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.ImageUrl)]
        public string Url { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime UpdateDate { get; set; }
        public long UserId { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime? DeleteTime { get; set; }
        public byte IsPublish { get; set; }
        public byte IsBlock { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}