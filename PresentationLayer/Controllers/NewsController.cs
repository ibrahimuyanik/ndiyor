using BusinessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using PresentationLayer.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PresentationLayer.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using X.PagedList;

namespace PresentationLayer.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService  _newsService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly Context _context;
        private readonly UserManager<AppUser> _userManager;


        public NewsController(INewsService newsService, UserManager<AppUser> userManager, Context context, ICloudStorageService cloudStorageService)
        {
            _newsService = newsService;
            _userManager = userManager;
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

        [HttpGet]
        public async Task<IActionResult> Index(int categoryId, int page = 1, int pageSize = 10)
        {
            var result = _newsService.GetNewsesByCategoryId(categoryId);

            if (result.Success)
            {
                var newsList = result.Data;

                // Toplam gönderi sayısı
                int totalItemCount = newsList.Count;

                // 1. sayfada gösterilecek maksimum gönderi sayısı
                int firstPageItemCount = 10;

                // Sayfa sayısını hesapla
                int totalPages = (int)Math.Ceiling((double)(totalItemCount - firstPageItemCount) / pageSize) + 1;

                // Sayfalama nesnesini oluştur
                IPagedList<News> pagedList;

                if (page == 1)
                {
                    pagedList = new StaticPagedList<News>(newsList.Take(firstPageItemCount), page, firstPageItemCount, firstPageItemCount);
                    foreach (var eat in pagedList)
                    {
                        await GenerateSignedUrl(eat);
                    }
                }
                else
                {
                    int startIndex = firstPageItemCount + (page - 2) * pageSize;
                    var pagedNews = newsList.Skip(startIndex).Take(pageSize);
                    foreach (var eat in pagedNews)
                    {
                        await GenerateSignedUrl(eat);
                    }
                    pagedList = new StaticPagedList<News>(pagedNews, page, pageSize, totalItemCount - firstPageItemCount);
                }

                ViewBag.CategoryId = categoryId;
                ViewBag.TotalPages = totalPages;

                

                return View(pagedList);
            }

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int newsId)
        {
            var result =  _newsService.GetNewsDetailById(newsId);
            //var result = _newsService.GetNewsDetailBySlug(slug);

            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                TempData["userId"] = user.Id;
            }
            
            
            

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


        [HttpGet]
        public async Task<IActionResult> CommentList(int newsId)
        {
            Context context = new Context();
            var comment = context.Comments.Include(x=> x.AppUser).Where(x => x.NewsID == newsId).OrderByDescending(x => x.ReleaseDate).ToList();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var model = new List<CommentListViewModel>();

            foreach (var item in comment)
            {
                var c = new CommentListViewModel
                {
                    AppUserID = Convert.ToInt32(item.AppUserID),
                    CommentID = item.CommentID,
                    Commentt = item.Commentt,
                    EatID = Convert.ToInt32(item.EatID),
                    NameSurname = item.AppUser.Name + " " + item.AppUser.Surname,
                    NewsID = Convert.ToInt32(item.NewsID),
                    ReleaseDate = Convert.ToDateTime(item.ReleaseDate),
                    UserName = item.AppUser.UserName
                };
                model.Add(c);
            }

            string json = System.Text.Json.JsonSerializer.Serialize(model);
            return Json(json);
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {

            Context context = new Context();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            comment.ReleaseDate = DateTime.Now;
            comment.AppUserID = user.Id;

            context.Comments.Add(comment);
            context.SaveChanges();
            return NoContent();
            //return RedirectToAction("CommentList");
        }


        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id)
        {
            Context context = new Context();
            var value = context.Comments.Find(id);
            context.Comments.Remove(value);
            context.SaveChanges();
            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateComment(int id, string updatedComment)
        {
            Context context = new Context();
            var comment = context.Comments.Find(id);

            if (comment != null)
            {
                comment.Commentt = updatedComment;
                
                context.SaveChanges();
                return NoContent();
            }

            return NotFound();
        }

    }
}
