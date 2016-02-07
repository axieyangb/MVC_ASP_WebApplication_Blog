using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BlogTest.Models
{
    [Table("ArticleComment")]
    public class ArticleComment
    {
        [Key]
        [Column("CommentID")]
        public long? CommentId { set; get; }
        [Column("CommenterID")]
        public long CommenterId { get; set; }
        [Column("ArticleID")]
        public long ArticleId { get; set; }
        [Column("ReplyID")]
        public long? ReplyId { set; get;}
        [Column("CreateDate")]
        public DateTime CreateDate { set; get;}
        [Column("isValid")]
        public int IsValid { set; get; }
        [AllowHtml]
        [DataType(DataType.Text)]
        [Column("Content")]
        public string Content { set; get; }
        [Column("IPAddress")]
        public string IpAddress { set; get; }
    }

}