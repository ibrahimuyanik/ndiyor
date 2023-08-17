using BusinessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PresentationLayer.Models;
using PresentationLayer.Services;
using System.Text.Json.Serialization;
using System.Text.Json;
using X.PagedList;

namespace PresentationLayer.Controllers
{
    public class EatController : Controller
    {
        private readonly IEatService _eatService;
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly Context _context;



        public EatController(IEatService eatService, ICommentService commentService, UserManager<AppUser> userManager, ICloudStorageService cloudStorageService, Context context)
        {
            _eatService = eatService;
            _commentService = commentService;
            _userManager = userManager;
            _cloudStorageService = cloudStorageService;
            _context = context;
        }
        private async Task GenerateSignedUrl(Eat eat)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(eat.SavedFileName))
            {
                eat.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(eat.SavedFileName);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = _eatService.GetAll();
            

            if (result.Success)
            {
                var eatList = result.Data;

                // Toplam gönderi sayısı
                int totalItemCount = eatList.Count;

                // Sayfa sayısını hesapla
                int totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

                // Sayfalama nesnesini oluştur
                IPagedList<Eat> pagedList;
            
                // İlgili sayfadaki gönderileri al
                var pagedEats = eatList.Skip((page - 1) * pageSize).Take(pageSize);
                foreach (var eat in pagedEats) 
                {
                    await GenerateSignedUrl(eat);
                }
                pagedList = new StaticPagedList<Eat>(pagedEats, page, pageSize, totalItemCount);

                ViewBag.TotalPages = totalPages;


                return View(pagedList);
            }
            return View();

        }




        [HttpGet]
        public async Task<IActionResult> Detail(int eatId)
        {
            Context context = new Context();
            var deneme = _eatService.GetByIdAll(eatId);

            var altComment = context.Comments.Where(x => x.TopCommentId != null).ToList();

            var result = _eatService.GetById(eatId);
            var getall = _eatService.GetAll();
            var comment = _commentService.GetById(eatId);
            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                TempData["userId"] = user.Id;
            }
            
            TempData["eatId"] = eatId;
            

            if (result.Success && getall.Success)
            {
                var viewModel = new EatDetailViewModel
                {
                    Eat = deneme.Data,
                    Eats = getall.Data,
                    //Comments = comments.Data
                    TopComments = altComment

                };
                if (!string.IsNullOrWhiteSpace(result.Data.SavedFileName))
                {
                    string imageUrl = await _cloudStorageService.GetSignedUrlAsync(result.Data.SavedFileName);
                    viewModel.SignedUrl = imageUrl;
                }
                return View(viewModel);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CommentList(int eatId)
        {
            Context context = new Context();
            
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var comment = context.Comments.Include(x=> x.AppUser).Where(x => x.EatID == eatId).OrderByDescending(x => x.ReleaseDate).ToList();



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
                    ReleaseDate = Convert.ToDateTime(item.ReleaseDate.Value.ToShortDateString()),
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
                comment.ReleaseDate = DateTime.Now;
                context.SaveChanges();
                return NoContent();
            }

            return NotFound();
        }

       


    }
}
