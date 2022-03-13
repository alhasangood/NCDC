using FluentValidation;

namespace Portal.ViewModels;

public class PasswordResetVM
{
    public string Password { get; set; } = default!;
}
public class PasswordResetVMValidator : AbstractValidator<PasswordResetVM>
{
    public PasswordResetVMValidator()
    {
        RuleFor(m => m.Password).NotEmpty().WithMessage("يجب ادخال كلمة المرور").WithErrorCode("VE9901");
        RuleFor(m => m.Password).Must(Validation.IsPassword).WithMessage("يرجي ادخال كلمة مرور معقدة").WithErrorCode("VE2121");
        RuleFor(m => m.Password).MaximumLength(8).WithMessage("طول كلمة المرور لا يقل عن 8 حروف").WithErrorCode("VE2122");
        RuleFor(m => m.Password).MaximumLength(20).WithMessage("طول كلمة المرور لا يزيد عن 20 حرف").WithErrorCode("VE2122");
    }
}
