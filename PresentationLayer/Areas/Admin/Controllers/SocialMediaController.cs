using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Const;
using System.Data;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class SocialMediaController : Controller
    {
        private readonly ISocialMediaService _socialMediaService;

        public SocialMediaController(ISocialMediaService socialMediaService)
        {
            _socialMediaService = socialMediaService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var result = _socialMediaService.GetAll();
            if (result.Success)
            {
                return View(result.Data);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddSocial()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSocial(SocialMedia socialMedia)
        {
            _socialMediaService.Add(socialMedia);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteSocial(int id)
        {
            var value = _socialMediaService.GetById(id);
            _socialMediaService.Delete(value.Data);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditSocial(int id)
        {
            var result = _socialMediaService.GetById(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return View();

        }
        [HttpPost]
        public IActionResult EditSocial(SocialMedia socialMedia)
        {
            _socialMediaService.Update(socialMedia);
            return RedirectToAction("Index");
        }
    }
}
