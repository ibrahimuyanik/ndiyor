using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class TestResultViewModel
    {
        public string TestName { get; set; } // Test adı
        public string ResultText { get; set; } // Sonuç mesajı
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public List<QuestionViewModel> Questions { get; set; }

        // Diğer ek bilgiler veya gereksinimlere göre özellikler eklenebilir.
    }
}
