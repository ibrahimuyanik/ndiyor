using BusinessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PresentationLayer.ViewComponents.Default
{
    public class _AdminContactPartial: ViewComponent
    {
        private readonly IContactService _contactService;
        Context context=new Context();
        public _AdminContactPartial(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IViewComponentResult Invoke()
        {
            var result = _contactService.GetAll();
            ViewBag.count = context.Contacts.Count();
            if (result.Success)
            {
                return View(result.Data);
               
            }
            return View();
        }
    }
}
