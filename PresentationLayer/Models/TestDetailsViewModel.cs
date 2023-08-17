using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class TestDetailsViewModel
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string TestCategoryName { get; set; }
        public List<QuestionViewModel> Sorular { get; set; }
        
    }
}
