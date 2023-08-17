using EntityLayer.Concrete;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class QuestionViewModel
    {
        public int? QuestionID { get; set; }
        public int? TestCategoryID { get; set; }
        public string? Content { get; set; }
        public List<ImageViewModel>? Images { get; set; }
        public List<QuestionOption> QuestionOptions { get; set; }

        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? SavedUrl { get; set; }

        [NotMapped]
        public string? SignedUrl { get; set; }
        public string? SavedFileName { get; set; }

        public string QuestionText { get; set; }
        public string SelectedOption { get; set; }
        public string CorrectOption { get; set; }
    }
}
