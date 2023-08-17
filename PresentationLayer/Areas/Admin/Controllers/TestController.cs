using BusinessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Const;
using PresentationLayer.Services;
using System.Data;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;
        private readonly ITestCategoryService _testCategoryService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly Context _context;

        public TestController(ITestService testService, ITestCategoryService testCategoryService, Context context, ICloudStorageService cloudStorageService)
        {
            _testService = testService;
            _testCategoryService = testCategoryService;
            _context = context;
            _cloudStorageService = cloudStorageService;
        }

        private async Task GenerateSignedUrl(Test test)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(test.SavedFileName))
            {
                test.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(test.SavedFileName);
            }
        }

        private string? GenerateFileNameToSave(string incomingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(incomingFileName);
            var extension = Path.GetExtension(incomingFileName);
            return $"{fileName}-{DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss")}{extension}";
        }

        private async Task ReplacePhoto(Test test)
        {
            if (test.Photo != null)
            {
                //replace the file by deleting referee.SavedFileName file and then uploading new referee.Photo
                if (!string.IsNullOrEmpty(test.SavedFileName))
                {
                    await _cloudStorageService.DeleteFileAsync(test.SavedFileName);
                }
                test.SavedFileName = GenerateFileNameToSave(test.Photo.FileName);
                test.SavedUrl = await _cloudStorageService.UploadFileAsync(test.Photo, test.SavedFileName);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = _context.Tests.Include(x => x.TestCategory).ToList();
            foreach (var item in result)
            {
                await GenerateSignedUrl(item);
            }
            return View(result);
        }

        [HttpGet]
        public IActionResult AddTest()
        {
            var result = _testCategoryService.GetAll();
            List<SelectListItem> valueTestCategory = (from x in result.Data
                                                  select new SelectListItem
                                                  {
                                                      Text = x.TestCategoryName,
                                                      Value = x.TestCategoryID.ToString()
                                                  }).ToList();
            ViewBag.listTestCategory = valueTestCategory;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTest([Bind("TestID,Title,TestCategoryID,Photo,SavedUrl,SavedFileName")] Test test)
        {
            if (test.Photo != null)
            {
                test.SavedFileName = GenerateFileNameToSave(test.Photo.FileName);
                test.SavedUrl = await _cloudStorageService.UploadFileAsync(test.Photo, test.SavedFileName);
            }
            _testService.Add(test);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteTest(int id)
        {
            var value = _testService.GetById(id);
            _testService.Delete(value.Data);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> EditTest(int id)
        {
            var result = _testCategoryService.GetAll();

            List<SelectListItem> editTestCategory = (from x in result.Data
                                                 select new SelectListItem
                                                 {
                                                     Text = x.TestCategoryName,
                                                     Value = x.TestCategoryID.ToString()
                                                 }).ToList();

            ViewBag.editTestCategory = editTestCategory;

            var testValue = _testService.GetById(id);
            await GenerateSignedUrl(testValue.Data);
            return View(testValue.Data);


        }
        [HttpPost]
        public async Task<ActionResult> EditTest([Bind("TestID,Title,TestCategoryID,Photo,SavedUrl,SavedFileName")] Test test)
        {
            await ReplacePhoto(test);
            _testService.Update(test);
            return RedirectToAction("Index");
        }
    }
}
