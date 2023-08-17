using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Const;
using System.Data;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConsts.Admin}")]
    public class NotificationsController : Controller
	{
		private readonly IContactService _contactSerivce;

		public NotificationsController(IContactService contactSerivce)
		{
			_contactSerivce = contactSerivce;
		}

		public IActionResult Index()
		{
			return View();
		}
		public PartialViewResult PartialNotific()
		{
			//var p = _contactSerivce.GetAll();
			ViewBag.p = "deneme";
			return PartialView();
		}
	}
}
