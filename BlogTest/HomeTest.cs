using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Blog;
using Blog.Controllers;
using Blog.Models;
using NUnit.Framework;

namespace BlogTest
{
    [TestFixture]
    public class HomeTest
    {
        private HomeController _home;
        [SetUp]
        public void Initial()
        {
           _home = new HomeController(@"Server=JERRYXIE\LOCAL;Database=Blog;uid=sa;password=a6163484a");
        }
        [Test]
        public void TestHomeIndex()
        {
            var actResult = _home.Index(null) as ViewResult;
            Assert.IsNotNull(actResult);
            var viewModel = actResult.ViewData.Model as IEnumerable<ArticleAbstract>;
            Assert.IsNotNull(viewModel);
            var enumerable = viewModel as ArticleAbstract[] ?? viewModel.ToArray();
            Assert.LessOrEqual(enumerable.Length,10);
            var orderedEnumerable = enumerable.OrderBy(a => a.PostDate ).ToArray();
            //Test Wether it is ordered
            for (var i = 0; i < enumerable.Length; i++)
            {
                Assert.AreEqual(enumerable[i].ArticleId,orderedEnumerable[enumerable.Length-1-i].ArticleId);
            }
            foreach (var one in enumerable)
            {
                Assert.IsNotNull(one.ArticleId);
                Assert.IsNotNull(one.AuthorId);
                Assert.IsNotNull(one.Title);
                Assert.IsNotNull(one.SubTitle);
                Assert.IsNotNull(one.AuthorName);
                Assert.IsNotNull(one.PostDate);
            }
        }
        [Test]
        public void TestPartialHomeIndex()
        {
            const int currentPage = 1;
            const int lastPage = 1;
            _home.Index(null);
            var actResult = _home.Index(currentPage, lastPage, null) as PartialViewResult;
            Assert.IsNotNull(actResult);
            var viewModel = actResult.ViewData.Model as IEnumerable<ArticleAbstract>;
            Assert.IsNotNull(viewModel);
            var enumerable = viewModel as ArticleAbstract[] ?? viewModel.ToArray();
            Assert.LessOrEqual(enumerable.Length, 10);
            var orderedEnumerable = enumerable.OrderBy(a => a.PostDate).ToArray();
            //Test Wether it is ordered
            for (var i = 0; i < enumerable.Length; i++)
            {
                Assert.AreEqual(enumerable[i].ArticleId, orderedEnumerable[enumerable.Length - 1 - i].ArticleId);
            }
            foreach (var one in enumerable)
            {
                Assert.IsNotNull(one.ArticleId);
                Assert.IsNotNull(one.AuthorId);
                Assert.IsNotNull(one.Title);
                Assert.IsNotNull(one.SubTitle);
                Assert.IsNotNull(one.AuthorName);
                Assert.IsNotNull(one.PostDate);
            }
        }
    }
}
