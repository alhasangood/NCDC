using FluentValidation;

namespace Portal.ViewModels
{
    public class AnalysisTypesFilterVM
    {
            public string AnalysisTypeName { get; set; }          
        }


        public class AnalysisTypesFilterVMValidator : AbstractValidator<AnalysisTypesFilterVM>
        {
            public AnalysisTypesFilterVMValidator()
            {
                RuleFor(m => m.AnalysisTypeName).NotEmpty().WithMessage("يرجي تعبئة اسم التحليل").WithErrorCode("VE0501");
              
            }
        }
    
}
