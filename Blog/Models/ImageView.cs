using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
    [Table("ImageView")]
    public class ImageView
    {
        [Key]
      public  long ImageID { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.ImageUrl)]
        public string Url { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime UpdateDate { get; set; }
        public long UserID { get; set; }
    }
}