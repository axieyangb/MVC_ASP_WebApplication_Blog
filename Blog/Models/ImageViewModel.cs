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
      public  long ImageID { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.ImageUrl)]
        public string Url { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime UpdateDate { get; set; }
        public long UserID { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime? DeleteTime { get; set; }
        public byte isPublish { get; set; }
        public byte isBlock { get; set; }
    }
}