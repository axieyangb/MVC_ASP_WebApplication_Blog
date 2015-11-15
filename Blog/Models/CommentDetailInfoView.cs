using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
    [Table("CommentDetailInfoView")]
    public class CommentDetailInfoView
    {
        [Key]
        public long CommentID { set; get; }
        public long CommenterID { get; set; }
        public long ArticleID { get; set; }
        public long ReplyID { set; get; }
        public DateTime CreateDate { set; get; }
        public string CommentName {set;get;}
        public string Content { set; get; }
    }
}