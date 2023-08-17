using BusinessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Const;
using System.Data;
using System.Drawing.Printing;
using X.PagedList;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        Context c = new Context();
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index( int page = 1, int pageSize = 10)
        {
            var result = _contactService.GetAll();
            if (result.Success)
            {
                var contactList = result.Data;

                // Toplam gönderi sayısı
                int totalItemCount = contactList.Count;

                // Sayfa sayısını hesapla
                int totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

                // Sayfalama nesnesini oluştur
                IPagedList<Contact> pagedList;

                // İlgili sayfadaki gönderileri al
                var pagedContact = contactList.Skip((page - 1) * pageSize).Take(pageSize);
               
                pagedList = new StaticPagedList<Contact>(pagedContact, page, pageSize, totalItemCount);

                ViewBag.TotalPages = totalPages;


                return View(pagedList);
            }
            return View();
        }
        public IActionResult DeleteContact(int id)
        {
            
            var value = _contactService.GetById(id);
            _contactService.Delete(value.Data);
            return RedirectToAction("Index");
        }
        //public async Task<IActionResult> ContactPartial()
        //{
            
        //    return PartialView("_ContactPartial");
        //}
       
    }
}
