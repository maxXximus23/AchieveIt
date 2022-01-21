using System;
using System.Net.Mail;
using AchieveIt.API.Models;
using FluentValidation;

namespace AchieveIt.API.Validators
{
    public class TeacherValidator : AbstractValidator<RegisterTeacherModel>
    {
        public TeacherValidator()
        {
            RuleFor(teacher => teacher.Email)
                .Must(email => MailAddress.TryCreate(email, out _)).WithMessage("Field must contain email address.")
                .NotEmpty().WithMessage("Email is required.");
            RuleFor(teacher => teacher.Name)
                .NotEmpty().WithMessage("Name is required.");
            RuleFor(teacher => teacher.Group)
                .NotEmpty().WithMessage("Group is required.");
            RuleFor(teacher => teacher.Password)
                .MinimumLength(4)
                .MaximumLength(32)
                .NotEmpty();
            RuleFor(teacher => teacher.Surname)
                .NotEmpty().WithMessage("Surname is required.");
            RuleFor(teacher => teacher.Patronymic)
                .NotEmpty().WithMessage("Patronymic is required.");
            RuleFor(teacher => teacher.Birthday)
                .NotEmpty().WithMessage("Birthday is required.")
                .LessThan(DateTime.UtcNow.Date);
        }
    }
}