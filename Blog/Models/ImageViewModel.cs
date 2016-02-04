using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
    [Table("ImageView")]
    public class ImageViewModel
    {
        [Key]
        public long ImageId { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Url { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime UpdateDate { get; set; }
        public long UserId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DeleteTime { get; set; }
        public byte IsPublish { get; set; }
        public byte IsBlock { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}