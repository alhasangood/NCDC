using FluentValidation;

namespace Portal.ViewModels
{
    public class ResultTypeVM
    {
        public string ResultTypeName { get; set; }
    }


    public class ResultTypeVMValidator : AbstractValidator<ResultTypeVM>
    {
        public ResultTypeVMValidator()
        {
            RuleFor(m => m.ResultTypeName).NotEmpty().WithMessage("يرجي تعبئة اسم التحليل").WithErrorCode("VE0501");

        }
    }
}
