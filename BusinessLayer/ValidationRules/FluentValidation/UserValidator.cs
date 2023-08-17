using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<AppUser>
    {
        public UserValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim boş geçilemez");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyisim boş geçilemez");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş geçilemez");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email boş geçilemez");
            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("Şifre boş geçilemez");

            RuleFor(x => x.Name).MinimumLength(2).WithMessage("İsim minimum 2 karakter olmaslı");
            RuleFor(x => x.Surname).MinimumLength(2).WithMessage("Soyisim minimum 2 karakter olmaslı");
            RuleFor(x => x.UserName).MinimumLength(4).WithMessage("Soyisim minimum 4 karakter olmaslı");
            RuleFor(x => x.PasswordHash).MinimumLength(6).WithMessage("Şifre minimum 6 karakter olmalı");

            RuleFor(x => x.ConfirmCode).Equal(x => x.ConfirmCode).WithMessage("Geçersiz Kod");
        }
    }
}
