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
using X.PagedList;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly Context _context;

        public NewsController(INewsService newsService, ICategoryService categoryService, Context context, ICloudStorageService cloudStorageService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _context = context;
            _cloudStorageService = cloudStorageService;
        }

        private async Task GenerateSignedUrl(News news)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(news.SavedFileName))
            {
                news.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(news.SavedFileName);
            }
        }

        private string? GenerateFileNameToSave(string incomingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(incomingFileName);
            var extension = Path.GetExtension(incomingFileName);
            return $"{fileName}-{DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss")}{extension}";
        }

        private async Task ReplacePhoto(News news)
        {
            if (news.Photo != null)
            {
                //replace the file by deleting referee.SavedFileName file and then uploading new referee.Photo
                if (!string.IsNullOrEmpty(news.SavedFileName))
                {
                    await _cloudStorageService.DeleteFileAsync(news.SavedFileName);
                }
                news.SavedFileName = GenerateFileNameToSave(news.Photo.FileName);
                news.SavedUrl = await _cloudStorageService.UploadFileAsync(news.Photo, news.SavedFileName);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            //var result = _newsService.GetAll();
            var result = _context.Newses.Include(x => x.Category).ToList();
           
                var newList = result;

                // Toplam gönderi sayısı
                int totalItemCount = newList.Count;

                // Sayfa sayısını hesapla
                int totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

                // Sayfalama nesnesini oluştur
                IPagedList<News> pagedList;

                // İlgili sayfadaki gönderileri al
                var pagedNews = newList.Skip((page - 1) * pageSize).Take(pageSize);
                foreach (var news in pagedNews)
                {
                    await GenerateSignedUrl(news);
                }
                pagedList = new StaticPagedList<News>(pagedNews, page, pageSize, totalItemCount);

                ViewBag.TotalPages = totalPages;
                
            
            foreach (var item in pagedList)
            {
                await GenerateSignedUrl(item);
            }
            return View(pagedList);
        }
        [HttpGet]
        public IActionResult AddNews()
        {
            var result = _categoryService.GetAll();
            List<SelectListItem> valueCategory = (from x in result.Data
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString()
                                                  }).ToList();
            ViewBag.listCategory = valueCategory;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNews([Bind("NewsID,Title,Content,CategoryID,Photo,SavedUrl,SavedFileName")] News news)
        {
            if (news.Photo != null)
            {
                news.SavedFileName = GenerateFileNameToSave(news.Photo.FileName);
                news.SavedUrl = await _cloudStorageService.UploadFileAsync(news.Photo, news.SavedFileName);
            }
            news.ReleaseDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            news.NewsStatus = true;
            _newsService.Add(news);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteNews(int id)
        {
            var value = _newsService.GetById(id);
            _newsService.Delete(value.Data);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> EditNews(int id)
        {
            var result = _categoryService.GetAll();
            var news = _newsService.GetAll();
            List<SelectListItem> editCategory = (from x in result.Data
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString()
                                                 }).ToList();

            ViewBag.editCategory = editCategory;

            var newValue = _newsService.GetById(id);
            await GenerateSignedUrl(newValue.Data);
            return View(newValue.Data);


        }
        [HttpPost]
        public async Task<ActionResult> EditNews([Bind("NewsID,Title,Content,CategoryID,Photo,SavedUrl,SavedFileName")] News news)
        {
            await ReplacePhoto(news);
            news.NewsStatus = true;  
            _newsService.Update(news);
            return RedirectToAction("Index");
        }
        public ActionResult EditorStatus(int id)
        {
            var editorValue = _newsService.GetById(id);
            if (editorValue.Data.EditorPickStatus == false)
            {

                editorValue.Data.EditorPickStatus = true;
                _newsService.Update(editorValue.Data);
                return RedirectToAction("Index");
            }
            else
            {
                editorValue.Data.EditorPickStatus = false;
                _newsService.Update(editorValue.Data);
                return RedirectToAction("Index");
            }
        }
        public ActionResult HotNewsStatus(int id)
        {
            var editorValue = _newsService.GetById(id);
            if (editorValue.Data.HotNewsStatus == false)
            {

                editorValue.Data.HotNewsStatus = true;
                _newsService.Update(editorValue.Data);
                return RedirectToAction("Index");
            }
            else
            {
                editorValue.Data.HotNewsStatus = false;
                _newsService.Update(editorValue.Data);
                return RedirectToAction("Index");
            }
        }
        
        public ActionResult NewsStatus(int id)
        {
            var newValue = _newsService.GetById(id);
            if (newValue.Data.NewsStatus == false)
            {

                newValue.Data.NewsStatus = true;
                _newsService.Update(newValue.Data);
                return RedirectToAction("Index");
            }
            else
            {
                newValue.Data.NewsStatus = false;
                _newsService.Update(newValue.Data);
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> NewsDetail(int id)
        {
            var result = _newsService.GetNewsDetailById(id);
            //var result = _newsService.GetNewsDetailBySlug(slug);

            if (result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.Data.SavedFileName))
                {
                    string imageUrl = await _cloudStorageService.GetSignedUrlAsync(result.Data.SavedFileName);
                    result.Data.SignedUrl = imageUrl;
                }
                return View(result.Data);
            }
            return View();

        }
    }
}
