using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Blog.Models
{
    public class ArticleSubmitView
    {
        [Required]
        [Column("AuthorID")]
        public long AuthorId { get; set; }
        [DataType(DataType.Text)]
        [Column("Title")]
        public string Title { get; set; }
        [DataType(DataType.Text)]
        [Column("SubTitle")]
        public string SubTitle { get; set; }
        [Required]
        [Column("Action")]
        public string Action { get; set; }
        [AllowHtml]
        [DataType(DataType.Text)]
        [Column("Content")]
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