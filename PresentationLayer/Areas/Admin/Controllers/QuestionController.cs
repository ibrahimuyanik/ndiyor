using BusinessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PresentationLayer.Const;
using PresentationLayer.Services;
using X.PagedList;
using static System.Net.Mime.MediaTypeNames;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =$"{RoleConsts.Admin}")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly ITestService _testService;
        private readonly ITestCategoryService _testCategoryService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly Context _context;

        public QuestionController(IQuestionService questionService, ITestService testService, ITestCategoryService testCategoryService, Context context, ICloudStorageService cloudStorageService)
        {
            _questionService = questionService;
            _testService = testService;
            _testCategoryService = testCategoryService;
            _context = context;
            _cloudStorageService = cloudStorageService;
        }


        private async Task GenerateSignedUrl(Question question)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(question.SavedFileName))
            {
                question.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(question.SavedFileName);
            }
        }

        private string? GenerateFileNameToSave(string incomingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(incomingFileName);
            var extension = Path.GetExtension(incomingFileName);
            return $"{fileName}-{DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss")}{extension}";
        }

        private async Task ReplacePhoto(Question question)
        {
            if (question.Photo != null)
            {
                //replace the file by deleting referee.SavedFileName file and then uploading new referee.Photo
                if (!string.IsNullOrEmpty(question.SavedFileName))
                {
                    await _cloudStorageService.DeleteFileAsync(question.SavedFileName);
                }
                question.SavedFileName = GenerateFileNameToSave(question.Photo.FileName);
                question.SavedUrl = await _cloudStorageService.UploadFileAsync(question.Photo, question.SavedFileName);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = _context.Questions.Include(x => x.TestCategory).Include(y=>y.Test).ToList();
            var questionList = result;

            // Toplam gönderi sayısı
            int totalItemCount = questionList.Count;

            // Sayfa sayısını hesapla
            int totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

            // Sayfalama nesnesini oluştur
            IPagedList<Question> pagedList;

            // İlgili sayfadaki gönderileri al
            var pagedQuestion = questionList.Skip((page - 1) * pageSize).Take(pageSize);
            
            pagedList = new StaticPagedList<Question>(pagedQuestion, page, pageSize, totalItemCount);

            ViewBag.TotalPages = totalPages;
           
            
            foreach (var item in result)
            {
                await GenerateSignedUrl(item);
            }
            return View(pagedList);

        }

        [HttpGet]
        public IActionResult AddQuestion()
        {
          
            List<SelectListItem> valueTestCategory = (from x in _testCategoryService.GetAll().Data
                                                      select new SelectListItem
                                                      {
                                                          Text = x.TestCategoryName,
                                                          Value = x.TestCategoryID.ToString()
                                                      }).ToList();

            List<SelectListItem> valueTest = (from x in _testService.GetAll().Data
                                                     select new SelectListItem
                                                     {
                                                         Text = x.Title,
                                                         Value = x.TestID.ToString()
                                                     }).ToList();
            ViewBag.listTestCategory = valueTestCategory;
            ViewBag.listTest = valueTest;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestion([Bind("QuestionID,Content,TestCategoryID,TestID,TrueAnswer,Photo,SavedUrl,SavedFileName")] Question question)
        {
            if (question.Photo != null)
            {
                question.SavedFileName = GenerateFileNameToSave(question.Photo.FileName);
                question.SavedUrl = await _cloudStorageService.UploadFileAsync(question.Photo, question.SavedFileName);
            }
            _questionService.Add(question);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteQuestion(int id)
        {
            var value = _questionService.GetById(id);
            _questionService.Delete(value.Data);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> EditQuestion(int id)
        {
            var data = _context.Questions.Include(z=> z.QuestionOptions).Where(y => y.QuestionID == id);
            var optionId = _context.QuestionOptions.Where(x => x.QuestionID == id);
            List<SelectListItem> editTestCategory = (from x in _testCategoryService.GetAll().Data
                                                 select new SelectListItem
                                                 {
                                                     Text = x.TestCategoryName,
                                                     Value = x.TestCategoryID.ToString()
                                                 }).ToList();
            List<SelectListItem> editTest = (from x in _testService.GetAll().Data
                                                     select new SelectListItem
                                                     {
                                                         Text = x.Title,
                                                         Value = x.TestID.ToString()
                                                     }).ToList();

            List<SelectListItem> trueAnswers = (from x in _context.QuestionOptions.Where(y => y.QuestionID == id).ToList()
                                                select new SelectListItem
                                                {
                                                    Text = x.Option,
                                                    Value = x.QuestionOptionID.ToString()
                                                }).ToList();
            ViewBag.editCategory = editTestCategory;
            ViewBag.editTest = editTest;
            ViewBag.trueAnswers = trueAnswers;

            var questValue = _questionService.GetById(id);
            await GenerateSignedUrl(questValue.Data);
            return View(questValue.Data);


        }
        [HttpPost]
        public async Task<ActionResult> EditQuestion(Question question)
        {
            await ReplacePhoto(question);

            _questionService.Update(question);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetQuesitonsDetailById(int id)
        {
            var result = _questionService.GetQuestionDetailById(id);
            return View(result.Data);

        }
    }
}
