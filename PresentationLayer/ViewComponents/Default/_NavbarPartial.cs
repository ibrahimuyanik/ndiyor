using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.ViewComponents.Default
{
    public class _NavbarPartial : ViewComponent
    {
        private readonly ITestCategoryService _testCategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IEatService _eatService;

        public _NavbarPartial(ITestCategoryService testCategoryService, ICategoryService categoryService, IEatService eatService)
        {
            _testCategoryService = testCategoryService;
            _categoryService = categoryService;
            _eatService = eatService;
        }

        public IViewComponentResult Invoke()
        {
            var resultTestCategory = _testCategoryService.GetAll();
            var resultCategory = _categoryService.GetAll();
            var resultEat = _eatService.GetAll();

            if (resultTestCategory.Success && resultCategory.Success && resultEat.Success)
            {
                var viewModel = new MyViewModel
                {
                    TestCategories = resultTestCategory.Data,
                    Categories = resultCategory.Data,
                    Eats = resultEat.Data
                };

                return View(viewModel);
            }
            return View();
        }
    }
}
