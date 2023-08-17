
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Const;
using System.Data;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class TestCategoryController : Controller
    {
        private readonly ITestCategoryService _testCategoryService;

        public TestCategoryController(ITestCategoryService testCategoryService)
        {
            _testCategoryService = testCategoryService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var result = _testCategoryService.GetAll();
            if (result.Success)
            {
                return View(result.Data);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddTestCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTestCategory(TestCategory testCategory)
        {
            _testCategoryService.Add(testCategory);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteTestCategory(int id)
        {
            var value = _testCategoryService.GetById(id);
            _testCategoryService.Delete(value.Data);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditTestCategory(int id)
        {
            var result = _testCategoryService.GetById(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return View();

        }
        [HttpPost]
        public IActionResult EditTestCategory(TestCategory testCategory)
        {
            _testCategoryService.Update(testCategory);
            return RedirectToAction("Index");
        }
    }
}
