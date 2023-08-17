using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.ViewComponents.Default
{
    public class _FooterPartial : ViewComponent
    {
        private readonly ISocialMediaService _socialMediaService;

        public _FooterPartial(ISocialMediaService socialMediaService)
        {
            _socialMediaService = socialMediaService;
        }
        public IViewComponentResult Invoke()
        {
            var result = _socialMediaService.GetAll();
            if (result.Success)
            {
                return View(result.Data);
            }
            return View();
        }
    }
}
