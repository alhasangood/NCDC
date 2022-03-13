using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using FluentValidation;

namespace Portal.ViewModels
{
    public class UserVM
    {
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public int UserType { get; set; }
        public string JobDescription { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public List<int> Permissions { get; set; }
    }


    public class UserVMValidator : AbstractValidator<UserVM>
    {
        public UserVMValidator()
        {
            RuleFor(m => m.LoginName).NotEmpty().WithMessage("يرجي تعبئة اسم الدخول").WithErrorCode("VE0501");
            RuleFor(m => m.LoginName).Must(Validation.IsEnglishAndNumbers).WithMessage("اسم الدخول يحتوي على حروف انجليزية فقط").WithErrorCode("VE0502");
            RuleFor(m => m.LoginName.Length).LessThan(50).WithMessage("اسم الدخول لا يزيد عن 50 حرف").WithErrorCode("VE0503");

            RuleFor(m => m.FullName).NotEmpty().WithMessage("يرجي تعبئة اسم المستخدم الكامل").WithErrorCode("VE0504");
            RuleFor(m => m.FullName).Must(Validation.IsArabicAndSpaces).WithMessage("اسم المستخدم الكامل يحتوي على حروف عربية فقط").WithErrorCode("VE0505");
            RuleFor(m => m.FullName.Length).LessThan(100).WithMessage("اسم المستخدم الكامل لا يزيد عن 100 حرف").WithErrorCode("VE0506");

            RuleFor(m => m.PhoneNo).NotEmpty().WithMessage("يرجي تعبئة رقم الهاتف").WithErrorCode("VE0507");
            RuleFor(m => m.PhoneNo).Must(Validation.IsLibyanMobile).WithMessage("يجب ان يكون رقم الهاتف خاص بدولة ليبيا").WithErrorCode("VE0508");

            RuleFor(m => m.Email).NotEmpty().WithMessage("يرجي تعبئة البريد الالكتروني").WithErrorCode("VE0509");
            RuleFor(m => m.Email).EmailAddress().WithMessage("يرجي كتابة البريد الالكتروني بشكل صحيح").WithErrorCode("VE0510");
            RuleFor(m => m.Email.Length).LessThan(50).WithMessage("البريد الالكتروني لا يزيد عن 50 خانة").WithErrorCode("VE0511");

            RuleFor(m => m.JobDescription).NotEmpty().WithMessage("يرجي تعبئة الوصف الوظيفي").WithErrorCode("VE0512");
            RuleFor(m => m.JobDescription).Must(Validation.IsArabicAndSpaces).WithMessage("الوصف الوظيفي يحتوي على حروف عربية فقط").WithErrorCode("VE0513");
            RuleFor(m => m.JobDescription.Length).LessThan(100).WithMessage("الوصف الوظيفي لا يزيد عن 100 حرف").WithErrorCode("VE0514");

        }
    }



}
