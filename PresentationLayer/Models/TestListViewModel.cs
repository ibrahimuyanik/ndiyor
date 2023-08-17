using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class TestListViewModel
    {
        public int TestID { get; set; }
        public string Title { get; set; }
        public string TestCategoryName { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? SavedUrl { get; set; }

        [NotMapped]
        public string? SignedUrl { get; set; }
        public string? SavedFileName { get; set; }
    }
}
