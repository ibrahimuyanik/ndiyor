using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Services;

namespace PresentationLayer.ViewComponents.Default
{
    public class _HealthPartial : ViewComponent
    {
        private readonly INewsService _newsService;
        private readonly ICloudStorageService _cloudStorageService;


        public _HealthPartial(INewsService newsService, ICloudStorageService cloudStorageService)
        {
            _newsService = newsService;
            _cloudStorageService = cloudStorageService;
        }

        public async Task GenerateSignedUrl(News news)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(news.SavedFileName))
            {
                news.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(news.SavedFileName);
            }
        }

        public IViewComponentResult Invoke()
        {
            var result = _newsService.GetAll();
            foreach (var x in result.Data)
            {
                GenerateSignedUrl(x);
            }
            return View(result.Data);
        }
    }
}
