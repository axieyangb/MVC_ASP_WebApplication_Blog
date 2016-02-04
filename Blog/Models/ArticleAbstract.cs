using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
    public class ArticleAbstract
    {
        public long ArticleID { get; set; }
        public long? AuthorID { get; set; }
        public string Title { get; set; }
        public string SubTitle {get;set;}
        public DateTime PostDate { get; set; }
        public string AuthorName { get; set; }
    }
}