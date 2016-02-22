
using Blog.Controllers;
using NUnit.Framework;

namespace BlogTest
{
   public class ArticleTest
    {
        ArticleController _article;

        [SetUp]
        public void Initial()
        {
            _article=new ArticleController(@"Server=JERRYXIE\LOCAL;Database=Blog;uid=sa;password=a6163484a");
            _article.SetEmojis(@"C:\Users\axieyangb\Desktop\webApp\MVC_ASP_WebApplication_Blog\BlogTest\TestData\emoji\");
        }

        [Test]
        public void RegexEmojiTest()
        {
            string content = "{0.1};";
           string ret=_article.RegexEmojiReplace(content);
            Assert.AreNotEqual(content,ret);
        }
    }
}
