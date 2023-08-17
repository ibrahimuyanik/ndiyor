using BusinessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.Admin.Models;
using PresentationLayer.Const;
using PresentationLayer.Services;
using System.Data;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class QuestionOptionController : Controller
    {
        private readonly Context _context;
        private readonly ITestCategoryService _testCategoryService;
        private readonly IQuestionService _questionService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly ITestService _testService;
        

        public QuestionOptionController(Context context, ITestCategoryService testCategoryService, IQuestionService questionService, ICloudStorageService cloudStorageService, ITestService testService)
        {
            _context = context;
            _testCategoryService = testCategoryService;
            _questionService = questionService;
            _cloudStorageService = cloudStorageService;
            _testService = testService;
        }

        [HttpGet]
        public IActionResult Index()
        {


            List<SelectListItem> valueTest = (from x in _context.Questions.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = x.Content,
                                                  Value = x.QuestionID.ToString()
                                              }).ToList();
            ViewBag.listTest = valueTest;
            return View();
        }

        [HttpPost]
        public IActionResult Index(QuestionOption questionOption)
        {
            _context.QuestionOptions.Add(questionOption);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
