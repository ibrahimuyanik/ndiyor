using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class ImageViewModel
    {
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? SavedUrl { get; set; }

        [NotMapped]
        public string? SignedUrl { get; set; }
        public string? SavedFileName { get; set; }
    }
}
