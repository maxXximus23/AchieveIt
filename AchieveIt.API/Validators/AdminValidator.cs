using System.Net.Mail;
using AchieveIt.API.Models;
using FluentValidation;

namespace AchieveIt.API.Validators
{
    public class AdminValidator : AbstractValidator<RegisterAdminModel>
    {
        public AdminValidator()
        {
            RuleFor(admin => admin.Email)
                .Must(email => MailAddress.TryCreate(email, out _)).WithMessage("Field must contain email address.")
                .NotEmpty().WithMessage("Email is required.");
            RuleFor(admin => admin.Password)
                .MinimumLength(4)
                .MaximumLength(32)
                .NotEmpty();
        }
    }
}