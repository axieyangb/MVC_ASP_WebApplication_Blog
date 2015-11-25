using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Blog.Models
{
    public class BlogContext :DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<ImageViewModel> Images { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<CommentDetailInfoView> CommentDetailInfo { get; set; }
        public DbSet<PublicImageModel> PublicImages { get; set; }
    }
}