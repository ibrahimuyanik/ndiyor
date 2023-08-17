using BusinessLayer.Abstract;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using PresentationLayer.Services;
using X.PagedList;

namespace PresentationLayer.Controllers
{
    public class TestController : Controller
    {
        private readonly ITestService _testService;
        private readonly ITestAnswerService _testAnswerService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly UserManager<AppUser>  _userManager;
        private readonly Context _context;

        Context context = new Context();

        public TestController(ITestService testService, ITestAnswerService testAnswerService, UserManager<AppUser> userManager, ICloudStorageService cloudStorageService, Context context)
        {
            _testService = testService;
            _testAnswerService = testAnswerService;
            _userManager = userManager;
            _cloudStorageService = cloudStorageService;
            _context = context;
        }

        private async Task GenerateSignedUrl(Test test)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(test.SavedFileName))
            {
                test.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(test.SavedFileName);
            }
        }
        private async Task GenerateSignedUrl(TestListViewModel test)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(test.SavedFileName))
            {
                test.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(test.SavedFileName);
            }
        }

        private async Task GenerateSignedUrl(QuestionViewModel test)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(test.SavedFileName))
            {
                test.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(test.SavedFileName);
            }
        }

        private TestListViewModel MapToViewModel(Test test)
        {
            return new TestListViewModel
            {
                TestID = test.TestID,
                Title = test.Title,
                TestCategoryName = test.TestCategory.TestCategoryName,
                Photo = test.Photo,
                SavedFileName = test.SavedFileName,
                SavedUrl = test.SavedUrl,
                SignedUrl = test.SignedUrl
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index(int testId, int page = 1, int pageSize = 10)
        {
            var tests = context.Tests.ToList();
            var filteredTests = context.Tests.Include(x => x.TestCategory).Where(x => x.TestCategoryID == testId).ToList();

            // Total test count for pagination
            int totalItemCount = filteredTests.Count;

            // Items to display on the first page
            int firstPageItemCount = 10;

            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)(totalItemCount - firstPageItemCount) / pageSize) + 1;

            // Create a paged list
            IPagedList<TestListViewModel> pagedList;

            if (page == 1)
            {
                pagedList = new StaticPagedList<TestListViewModel>(
                    filteredTests.Take(firstPageItemCount).Select(test => MapToViewModel(test)),
                    page, firstPageItemCount, firstPageItemCount);
                foreach (var test in pagedList)
                {
                    await GenerateSignedUrl(test);
                }
            }
            else
            {
                int startIndex = firstPageItemCount + (page - 2) * pageSize;
                var pagedTests = filteredTests.Skip(startIndex).Take(pageSize);
                foreach (var test in pagedTests)
                {
                    await GenerateSignedUrl(test);
                }
                pagedList = new StaticPagedList<TestListViewModel>(
                    pagedTests.Select(test => MapToViewModel(test)),
                    page, pageSize, totalItemCount - firstPageItemCount);
                
            }

            ViewBag.TestId = testId;
            ViewBag.TotalPages = totalPages;

            return View(pagedList);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int testId)
        {

            var user = await _userManager.GetUserAsync(User);
            //ViewBag.UserId = user.Id;
            var test = context.Tests
         .Include(t => t.TestCategory)
         .Include(x => x.Images)
         .Include(t => t.Questions).ThenInclude(q => q.QuestionOptions)
         .FirstOrDefault(t => t.TestID == testId);

            if (test == null)
            {

                return NotFound();
            }

            var viewModel = new TestDetailsViewModel
            {
                TestId = test.TestID,
                TestName = test.Title,
                TestCategoryName = test.TestCategory.TestCategoryName,
                Sorular = test.Questions.Select(q => new QuestionViewModel
                {
                    QuestionID = q.QuestionID,
                    TestCategoryID = q.TestCategoryID,
                    Content = q.Content,
                    QuestionOptions = q.QuestionOptions.ToList(),
                    SignedUrl = q.SignedUrl,
                    SavedUrl = q.SavedUrl,
                    SavedFileName = q.SavedFileName,
                    Photo = q.Photo 
                }).ToList()
            };
            foreach (var question in viewModel.Sorular)
            {
                if (!string.IsNullOrWhiteSpace(question.SavedFileName))
                {
                    string imageUrl = await _cloudStorageService.GetSignedUrlAsync(question.SavedFileName);
                    question.SignedUrl = imageUrl;
                }
            }
            return View(viewModel);


        }


        [HttpPost]
        public IActionResult SubmitTest(TestSubmitViewModel viewModel)
        {
            //if (ModelState.IsValid)
            //{

            //        var test = context.Tests
            //            .Include(t => t.Questions).ThenInclude(q => q.QuestionOptions)
            //            .FirstOrDefault(t => t.TestID == viewModel.TestId);

            //    if (test == null)
            //    {
            //        return NotFound();
            //    }

            //    int correctAnswers = 0;
            //    foreach (var questionId in viewModel.Answers.Keys)
            //    {
            //        var selectedOptionId = viewModel.Answers[questionId];
            //        var question = test.Questions.FirstOrDefault(q => q.QuestionID == questionId);
            //        if (question != null)
            //        {
            //            var correctOption = question.QuestionOptions.FirstOrDefault(o => o.QuestionOptionID == selectedOptionId && selectedOptionId == question.TrueAnswer);
            //            if (correctOption != null)
            //            {
            //                correctAnswers++;
            //            }
            //        }
            //    }

            //    var totalQuestions = test.Questions.Count;
            //    var score = (double)correctAnswers / totalQuestions * 100; 

            //    string resultText;
            //    if (score >= 50)
            //    {
            //        resultText = "Tebrikler! Testi başarıyla geçtiniz.";
            //    }
            //    else
            //    {
            //        resultText = "Üzgünüz, testi geçemediniz. Daha fazla çalışmanızı öneririz.";
            //    }

            //    var resultViewModel = new TestResultViewModel
            //    {
            //        TestName = test.Title,
            //        ResultText = resultText
            //    };

            //    return View("TestResult", resultViewModel);
            //}


            //return RedirectToAction("Details", new { id = viewModel.TestId });

            if (ModelState.IsValid)
            {
                var test = context.Tests
                    .Include(t => t.Questions).ThenInclude(q => q.QuestionOptions)
                    .FirstOrDefault(t => t.TestID == viewModel.TestId);

                if (test == null)
                {
                    return NotFound();
                }

                int correctAnswers = 0;
                var questionResultViewModels = new List<QuestionViewModel>();

                foreach (var questionId in viewModel.Answers.Keys)
                {
                    var selectedOptionId = viewModel.Answers[questionId];
                    var question = test.Questions.FirstOrDefault(q => q.QuestionID == questionId);
                    if (question != null)
                    {
                        var correctOption = question.QuestionOptions.FirstOrDefault(o => o.QuestionOptionID == selectedOptionId && selectedOptionId == question.TrueAnswer);
                        if (correctOption != null)
                        {
                            correctAnswers++;
                        }

                        var questionResultViewModel = new QuestionViewModel
                        {
                            QuestionText = question.Content,
                            SelectedOption = question.QuestionOptions.FirstOrDefault(o => o.QuestionOptionID == selectedOptionId)?.Option,
                            CorrectOption = question.QuestionOptions.FirstOrDefault(o => o.QuestionOptionID == question.TrueAnswer)?.Option
                        };

                        questionResultViewModels.Add(questionResultViewModel);
                    }
                }

                var totalQuestions = test.Questions.Count;
                var score = (double)correctAnswers / totalQuestions * 100;

                string resultText;
                if (score >= 50)
                {
                    resultText = "Tebrikler! Testi başarıyla geçtiniz.";
                }
                else
                {
                    resultText = "Üzgünüz, testi geçemediniz. Daha fazla çalışmanızı öneririz.";
                }

                var resultViewModel = new TestResultViewModel
                {
                    TestName = test.Title,
                    ResultText = resultText,
                    TotalQuestions = totalQuestions,
                    CorrectAnswers = correctAnswers,
                    Questions = questionResultViewModels
                };

                return View("TestResult", resultViewModel);
            }

            return RedirectToAction("Details", new { id = viewModel.TestId });

        }

        public IActionResult TestResult(string testName)
        {
            var resultViewModel = new TestResultViewModel
            {
                TestName = testName
                
            };

            return View(resultViewModel);
        }
    }
}