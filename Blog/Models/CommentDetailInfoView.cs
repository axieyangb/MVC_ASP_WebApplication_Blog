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
        public long CommentId { set; get; }
        public long CommenterId { get; set; }
        public long ArticleId { get; set; }
        public long ReplyId { set; get; }
        public DateTime CreateDate { set; get; }
        public string CommentName {set;get;}
        public string Country { set; get; }
        public string Content { set; get; }
    }
}