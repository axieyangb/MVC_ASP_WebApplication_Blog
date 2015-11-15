using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
    [Table("ArticleComment")]
    public class ArticleComment
    {
        [Key]
        public long CommentID { set; get; }
        public long CommenterID { get; set; }
        public long ArticleID { get; set; }
        public long ReplyID { set; get;}
        public DateTime CreateDate { set; get;}
        public int isValue { set; get; }
        public string Content { set; get; }
    }

}