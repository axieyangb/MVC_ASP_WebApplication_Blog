using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Blog.Models
{
    [Table("ArticleComment")]
    public class ArticleComment
    {
        [Key]
        public long? CommentId { set; get; }
        public long CommenterId { get; set; }
        public long ArticleId { get; set; }
        public long? ReplyId { set; get;}
        public DateTime CreateDate { set; get;}
        public int IsValid { set; get; }
        [AllowHtml]
        [DataType(DataType.Text)]
        public string Content { set; get; }
        public string IpAddress { set; get; }
    }

}