using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application
{
    public class UserValidator : AbstractValidator<UserForCreationDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage("Email address must be valid.");
            RuleFor(u => u.FirstName).MinimumLength(4).WithMessage("Firstname must be at least 4 characters.")
                .MaximumLength(20).WithMessage("Firstname must be at most 20 characters.");
            RuleFor(u => u.LastName).MinimumLength(4).WithMessage("Lastname must be at least 4 characters.")
                .MaximumLength(20).WithMessage("Lastname must be at most 20 characters.");
            RuleFor(u => u.UserName).MinimumLength(4).WithMessage("Username must be at least 4 characters.")
                .MaximumLength(20).WithMessage("Username must be at most 20 characters.");
        }
    }
}
