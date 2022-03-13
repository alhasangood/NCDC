using FluentValidation;

namespace Portal.ViewModels
{
    public class ResultTypesFilterVM
    {
        public string ResultTypeName { get; set; }
    }


    public class ResultTypesFilterVMValidator : AbstractValidator<ResultTypesFilterVM>
    {
        public ResultTypesFilterVMValidator()
        {
            RuleFor(m => m.ResultTypeName).NotEmpty().WithMessage("يرجي تعبئة اسم التحليل").WithErrorCode("VE0501");

        }
    }
}
