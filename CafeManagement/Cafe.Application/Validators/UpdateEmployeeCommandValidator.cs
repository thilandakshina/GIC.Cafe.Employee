using Cafe.Application.Commands.Employee;
using FluentValidation;
using static Cafe.Application.Common;

namespace Cafe.Application.Validators
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(6, 10);
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^[89]\d{7}$")
                .WithMessage("Phone number must start with 8 or 9 and have 8 digits");
            RuleFor(x => x.Gender)
            .IsInEnum()
            .Must(x => x == GenderType.Male || x == GenderType.Female)
            .WithMessage("Gender must be either Male or Female");
        }
    }
}
