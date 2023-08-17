using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Const;
using System.Data;

namespace PresentationLayer.Controllers
{
    public class DefaultController : Controller
    {
        //[Authorize(Roles = $"{RoleConsts.User}, {RoleConsts.Admin}")]
        public IActionResult Index()
        {  
            return View();
        }
    }
}
