using Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModels
{
    public class LoginVM
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Captcha { get; set; }

    }
    public class LoginVMValidator : AbstractValidator<LoginVM>
    {
        public LoginVMValidator()
        {
            RuleFor(a => a.LoginName).NotEmpty().WithMessage("اسم المستخدم مطلوب").WithErrorCode("VE0901");
            RuleFor(a => a.LoginName).Must(Validation.IsEnglishAndNumbers).WithMessage("الرجاء ادخال اسم الدخول بشكل صحيح").WithErrorCode("VE0902");
            RuleFor(a => a.Password).NotEmpty().WithErrorCode("كلمة المرور مطلوبة").WithErrorCode("VE0903");

            RuleFor(a => a.Captcha).NotEmpty().WithMessage("الرمز الذي ادخلته غير مطابق لرمز التحقق").WithErrorCode("VE0613");
            RuleFor(a => a.Captcha).Must(Validation.IsEnglishAndNumbers).WithMessage("الرمز الذي ادخلته غير مطابق لرمز التحقق").WithErrorCode("VE0614");
            RuleFor(a => a.Captcha).Length(5).WithMessage("الرمز الذي ادخلته غير مطابق لرمز التحقق").WithErrorCode("VE0615");

        }
    }
}
