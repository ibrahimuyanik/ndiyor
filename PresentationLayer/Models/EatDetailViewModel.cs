using EntityLayer.Concrete;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class EatDetailViewModel
    {
        public Eat Eat { get; set; }
        public List<Eat> Eats { get; set; }


        public List<Comment>? TopComments { get; set; }
        public Comment Comment { get; set; }
        public List<Comment> Comments { get; set; }

        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? SavedUrl { get; set; }

        [NotMapped]
        public string? SignedUrl { get; set; }
        public string? SavedFileName { get; set; }

    }
}
