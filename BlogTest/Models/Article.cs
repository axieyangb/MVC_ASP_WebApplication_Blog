using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BlogTest.Models
{
    [Table("Article")]
    public class Article
    {
        [Key]
        [Column("ArticleID")]
        public long ArticleId { get; set; }
        [Required]
        [Column("AuthorID")]
        public long AuthorId { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please provide Title", AllowEmptyStrings = false)]
        [Column("Title")]
        public string Title { get; set; }
        [DataType(DataType.Text)]
        [Column("SubTitle")]
        public string SubTitle { get; set; }
        [AllowHtml]
        [Column("Content")]
        [DataType(DataType.Text)]
        public string Content { get; set; }
        [Column("TagID_1")]
        public long? TagId1 { get; set; }
        [Column("TagID_2")]
        public long? TagId2 { get; set; }
        [Column("TagID_3")]
        public long? TagId3 { get; set; }
        [Column("TagID_4")]
        public long? TagId4 { get; set; }
        [Column("TagID_5")]
        public long? TagId5 { get; set; }
        [Column("PostDate")]
        public DateTime PostDate { get; set; }
        [Column("ModifyDate")]
        public DateTime? ModifyDate { get; set; }

    }
}