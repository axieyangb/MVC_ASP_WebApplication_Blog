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

        public long ArticleId { get; set; }

        public long? AuthorId { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public DateTime PostDate { get; set; }

        public string AuthorName { get; set; }
        public long? TagId1 { get; set; }
        public long? TagId2 { get; set; }
        public long? TagId3 { get; set; }
        public long? TagId4 { get; set; }
        public long? TagId5 { get; set; }
    }
}