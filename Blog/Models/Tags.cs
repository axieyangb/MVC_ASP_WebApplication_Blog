using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Blog.Models
{
    [Table("Tags")]
    public class Tags
    {
        [Key]
        public long? TagId { get; set; }
        [DataType(DataType.Text)]
        public string TagContent { get; set; }
        public int TagCount { get; set; }
        public DateTime? LastUsedDate { get; set; }
    }
}