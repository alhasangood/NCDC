using FluentValidation;

namespace Portal.ViewModels
{
    public class AnalysisTypeVM
    {
        public string AnalysisTypeName { get; set; }
        public string AnalysisTypeCode { get; set; }
        public string Description { get; set; }
      
    }


    public class AnalysisTypeVMValidator : AbstractValidator<AnalysisTypeVM>
    {
        public AnalysisTypeVMValidator()
        {
            RuleFor(m => m.AnalysisTypeName).NotEmpty().WithMessage("يرجي تعبئة اسم الدخول").WithErrorCode("VE0501");
            RuleFor(m => m.AnalysisTypeName.Length).LessThan(50).WithMessage("اسم الدخول لا يزيد عن 50 حرف").WithErrorCode("VE0503");

            RuleFor(m => m.AnalysisTypeCode).NotEmpty().WithMessage("يرجي تعبئة اسم المستخدم الكامل").WithErrorCode("VE0504");
            RuleFor(m => m.AnalysisTypeCode.Length).LessThan(100).WithMessage("اسم المستخدم الكامل لا يزيد عن 100 حرف").WithErrorCode("VE0506");        

            RuleFor(m => m.Description).NotEmpty().WithMessage("يرجي تعبئة الوصف الوظيفي").WithErrorCode("VE0512");
            RuleFor(m => m.Description).Must(Validation.IsArabicAndSpaces).WithMessage("الوصف الوظيفي يحتوي على حروف عربية فقط").WithErrorCode("VE0513");
            RuleFor(m => m.Description.Length).LessThan(100).WithMessage("الوصف الوظيفي لا يزيد عن 100 حرف").WithErrorCode("VE0514");

        }
    }
}
