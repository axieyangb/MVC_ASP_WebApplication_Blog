﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Blog.Models
{
     [Table("Article")]
    public class Article
    {
         [Key]
         public long ArticleID { get; set; }
         [Required]
         public long AuthorID { get; set; }
         [DataType(DataType.Text)]
         [Required(ErrorMessage = "Please provide Title", AllowEmptyStrings = false)]
        public string Title { get; set; }
         [DataType(DataType.Text)]
        public string SubTitle { get; set; }
        [AllowHtml]
        [DataType(DataType.Text)]
        public string Content { get; set; }
        public long? TagID_1 { get; set; }
        public long? TagID_2 { get; set; }
        public long? TagID_3 { get; set; }
        public long? TagID_4 { get; set; }
        public long? TagID_5 { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime? ModifyDate { get; set; }

    }
}