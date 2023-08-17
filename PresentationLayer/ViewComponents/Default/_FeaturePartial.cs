using BusinessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Services;

namespace PresentationLayer.ViewComponents.Default
{
    public class _FeaturePartial : ViewComponent
    {
        private readonly INewsService _newService;
        private readonly ICloudStorageService _cloudStorageService;


        public _FeaturePartial(INewsService newService, ICloudStorageService cloudStorageService)
        {
            _newService = newService;
            _cloudStorageService = cloudStorageService;
        }

        public async Task GenerateSignedUrl(NewsDetailDto news)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(news.SavedFileName))
            {
                news.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(news.SavedFileName);
            }
        }

        public IViewComponentResult Invoke()
        {
            var result = _newService.GetNewsDetails();
            foreach (var x in result.Data)
            {
                GenerateSignedUrl(x);
            }
            return View(result.Data);

        }
    }
}
