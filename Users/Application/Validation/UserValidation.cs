using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Entities.Dtos;

namespace Application.Validation
{
    public class UserValidation : AbstractValidator<UserForCreationDto>
    {
        public UserValidation()
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage("Email in an incorrect format.");
            RuleFor(u => u.FirstName).NotEmpty().WithMessage("FirstName can't be empty.").MaximumLength(20).WithMessage("FirstName can't be more than 20 symbols.");
            RuleFor(u => u.LastName).NotEmpty().WithMessage("LastName can't be empty.").MaximumLength(20).WithMessage("LastName can't be more than 20 symbols.");
            RuleFor(u => u.UserName).NotEmpty().WithMessage("UserName can't be empty.").MaximumLength(20).WithMessage("LastName can't be more than 20 symbols.");
            RuleFor(u => u.Password).NotEmpty().WithMessage("Password can't be empty.")
                .MinimumLength(8).WithMessage("Password can't be less than 8 symbols.")
                .MaximumLength(14).WithMessage("Password can't be more than 14 symbols.");
        }
    }
}
