using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Models
{
    public class CustomIdentityModel : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {

            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Parola çok kısa en az {length} karakter girişi yapınız"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresLower",
                Description = "Lütfen en az bir tane küçük harf giriniz"
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresUpper",
                Description = "Lütfen en az bir tan büyük harf giriniz"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresDigit",
                Description = "Lütfen en az bir tane sayı giriniz"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Lütfen en az bir tane sembol giriniz"
            };
        }
    }
}
