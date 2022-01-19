using System.Net.Mail;
using AchieveIt.API.Models;
using FluentValidation;

namespace AchieveIt.API.Validators
{
    public class SignInValidator : AbstractValidator<SignInModel>
    {
        public SignInValidator()
        {
            RuleFor(sign => sign.Email)
                .Must(email => MailAddress.TryCreate(email, out _)).WithMessage("Field must contain email address.")
                .NotEmpty().WithMessage("Email is required.");
            RuleFor(sign => sign.Password)
                .MinimumLength(4)
                .MaximumLength(32)
                .NotEmpty();
        }
    }
}