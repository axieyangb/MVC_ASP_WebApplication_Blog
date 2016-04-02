using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
    [Table("CommentDetail")]
    public class CommentDetailInfoView
    {
        [Key]
        [Column("CommentID")]
        public long CommentId { set; get; }
        [Column("CommenterID")]
        public long CommenterId { get; set; }
        [Column("ArticleID")]
        public long ArticleId { get; set; }
        [Column("Title")]
        public string Title { set; get; }
        [Column("ReplyID")]
        public long ReplyId { set; get; }
        [Column("CreateDate")]
        public DateTime CreateDate { set; get; }
        [Column("CommentName")]
        public string CommentName { set; get; }
        [Column("Country")]
        public string Country { set; get; }
        [Column("Content")]
        public string Content { set; get; }

        [Column("ReplyName")]
        public string ReplyName { set; get; }

        [Column("ReplyContent")]
        public string ReplyContent { get; set; }
    }
}