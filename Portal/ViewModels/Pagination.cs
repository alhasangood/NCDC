using FluentValidation;

namespace Portal.ViewModels
{
    public class Pagination
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public class PaginationValidator : AbstractValidator<Pagination>
    {
        public PaginationValidator()
        {
            RuleFor(m => m.Page).GreaterThan(0).WithMessage("الرجاء ادخال رقم الصفحة بشكل صحيح").WithErrorCode("VE9901");
            RuleFor(m => m.PageSize).GreaterThan(0).WithMessage("الرجاء ادخال حجم الصفحة بشكل صحيح").WithErrorCode("VE9902");
        }
    }
}