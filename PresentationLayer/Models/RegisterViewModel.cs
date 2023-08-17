using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "İsim alanı boş geçilemez!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyadı alanı boş geçilemez!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı boş geçilemez!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lütfen Şifreyi Tekrar Girin")]
        [Compare("Password", ErrorMessage = "Şifreler Uyumlu Değil")]
        public string ConfirmPassword { get; set; }
    }
}
