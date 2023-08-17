namespace PresentationLayer.Controllers
{
    public class TestSubmitViewModel
    {
        public int TestId { get; set; } // Test ID'si (Hidden input ile gönderilir)
        public string? AppUserID { get; set; }
        public Dictionary<int, int> Answers { get; set; } // Kullanıcının cevapları (Soru ID'si ve seçilen şık ID'si)

        
    }
}
