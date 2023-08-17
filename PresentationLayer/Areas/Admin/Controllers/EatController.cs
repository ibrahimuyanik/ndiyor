using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Const;
using PresentationLayer.Services;
using System.Data;
using System.Drawing.Printing;
using X.PagedList;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class EatController : Controller
    {
        private readonly IEatService _eatService;
        private readonly ICloudStorageService _cloudStorageService;

        public EatController(IEatService eatService, ICloudStorageService cloudStorageService)
        {
            _eatService = eatService;
            _cloudStorageService = cloudStorageService;
        }

        private async Task GenerateSignedUrl(Eat eat)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(eat.SavedFileName))
            {
                eat.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(eat.SavedFileName);
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
        //public async Task<IActionResult> DeleteEat(int id)
        //{
        //    var value = _eatService.GetById(id);
        //    await _cloudStorageService.DeleteFileAsync(value.Data.SavedFileName);
        //    _eatService.Delete(value.Data);
        //    return RedirectToAction("Index");
        //}
        [HttpGet]
        public IActionResult AddEat()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddEat([Bind("EatID,EatTitle,Content,Photo,SavedUrl,SavedFileName")] Eat eat)
        {

            if (eat.Photo != null)
            {
                eat.SavedFileName = GenerateFileNameToSave(eat.Photo.FileName);
                eat.SavedUrl = await _cloudStorageService.UploadFileAsync(eat.Photo, eat.SavedFileName);
            }
            eat.ReleaseDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            eat.EatStatus = true;
            _eatService.Add(eat);


            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> EditEat(int id)
        {
            var result = _eatService.GetById(id);
            if (result.Success)
            {
                await GenerateSignedUrl(result.Data);
                return View(result.Data);
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> EditEat([Bind("EatID,EatTitle,Content,Photo,SavedUrl,SavedFileName")] Eat eat)
        {
            await ReplacePhoto(eat);
            eat.EatStatus = true;
            _eatService.Update(eat);
            return RedirectToAction("Index");
        }

        private async Task ReplacePhoto(Eat eat)
        {
            if (eat.Photo != null)
            {
                //replace the file by deleting referee.SavedFileName file and then uploading new referee.Photo
                if (!string.IsNullOrEmpty(eat.SavedFileName))
                {
                    await _cloudStorageService.DeleteFileAsync(eat.SavedFileName);
                }
                eat.SavedFileName = GenerateFileNameToSave(eat.Photo.FileName);
                eat.SavedUrl = await _cloudStorageService.UploadFileAsync(eat.Photo, eat.SavedFileName);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EatDetail(int id)
        {
            var result = _eatService.GetEatDetailById(id);

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
        public ActionResult EatStatus(int id)
        {
            var eatValue = _eatService.GetById(id);
            if (eatValue.Data.EatStatus == false)
            {

                eatValue.Data.EatStatus = true;
                _eatService.Update(eatValue.Data);
                return RedirectToAction("Index");
            }
            else
            {
                eatValue.Data.EatStatus = false;
                _eatService.Update(eatValue.Data);
                return RedirectToAction("Index");
            }
        }


    }
}

