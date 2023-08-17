using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PresentationLayer.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Adını giriniz...")]
        public string Username { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifreyi giriniz...")]
        public string Password { get; set; }
    }
}
