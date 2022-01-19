using System;
using System.Net.Mail;
using AchieveIt.API.Models;
using AchieveIt.DataAccess.Entities;
using FluentValidation;

namespace AchieveIt.API.Validators
{
    public class StudentValidator : AbstractValidator<RegisterStudentModel>
    {
        public StudentValidator()
        {
            RuleFor(student => student.Email)
                .Must(email => MailAddress.TryCreate(email, out _)).WithMessage("Field must contain email address.")
                .NotEmpty().WithMessage("Email is required.");
            RuleFor(student => student.Name)
                .NotEmpty().WithMessage("Name is required.");
            RuleFor(student => student.Group)
                .NotEmpty().WithMessage("Group is required.");
            RuleFor(student => student.Password)
                .MinimumLength(4)
                .MaximumLength(32)
                .NotEmpty();
            RuleFor(student => student.Surname)
                .NotEmpty().WithMessage("Surname is required.");
            RuleFor(student => student.Patronymic)
                .NotEmpty().WithMessage("Patronymic is required.");
            RuleFor(student => student.Birthday)
                .NotEmpty().WithMessage("Birthday is required.")
                .LessThan(DateTime.UtcNow.Date);
        }
    }
}