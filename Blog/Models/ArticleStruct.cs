using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class CommentLevel
    {
        public CommentDetailInfoView ParentComment { get; set; }
      public  List<CommentDetailInfoView> ChildComments{get;set;}
    }
    public class ArticleStruct
    {
        public Article Article { set; get; }
        public ArticleComment CommentArticle { set; get; }
        public List<CommentLevel> RootComments { get; set; }
    }
}