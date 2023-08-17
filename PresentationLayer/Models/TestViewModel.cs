using EntityLayer.Concrete;
using EntityLayer.DTOs;

namespace PresentationLayer.Models
{
    public class TestViewModel
    {
        public int TestID { get; set; }
        public string Title { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}
