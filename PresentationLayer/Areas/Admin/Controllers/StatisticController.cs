using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Const;
using System.Data;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class StatisticController : Controller
    {
        Context c=new Context();
        public IActionResult Index()
        {
			var categoryTotal = c.Categories.Count().ToString();
			ViewBag.totalCategories = categoryTotal;

			var newsCount = c.Newses.Count().ToString();
			ViewBag.newsCount = newsCount;

			var eatCount = c.Eats.Count().ToString();
			ViewBag.eatCount = eatCount;

			var userCount = c.Users.Count().ToString();
			ViewBag.userCount = userCount;

			var testCount = c.Tests.Count().ToString();
			ViewBag.testCount = testCount;

			//var testCategoryCount = c.TestCategories.Count().ToString();
			//ViewBag.testCategoryCount = testCategoryCount;

			
			//En çok sorunn bulunduğu test kategorisini adı
			return View();
        }
    }
}
