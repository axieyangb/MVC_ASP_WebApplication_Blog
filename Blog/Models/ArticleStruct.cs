using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class CommentLevel
    {
        public CommentDetailInfoView parentComment { get; set; }
      public  List<CommentDetailInfoView> childComments{get;set;}
    }
    public class ArticleStruct
    {
        public Article article { set; get; }
        public List<CommentLevel> rootComments { get; set; }
    }
}