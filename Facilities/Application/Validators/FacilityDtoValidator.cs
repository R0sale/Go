using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Application.Dtos;

namespace Application.Validators
{
    public class FacilityDtoValidator : AbstractValidator<FacilityDto>
    {
        public FacilityDtoValidator()
        {
            RuleFor(f => f.Name).NotEmpty().WithMessage("Facility name is required.")
                .MaximumLength(100).WithMessage("Facility name must not exceed 100 characters.");

            RuleFor(f => f.Address).NotEmpty().WithMessage("Facility address is required.")

            RuleFor(f => f.OpeningHours).NotEmpty().WithMessage("Opening hours are required.")
                .Matches(@"^([01]\d|2[0-3]):[0-5]\d\s*-\s*([01]\d|2[0-3]):[0-5]\d$").WithMessage("Opening hours must be in the format HH:mm - HH:mm.");

            RuleFor(f => f.PhoneNumber).Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid international format.");

            RuleFor(f => f.Email).EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(25).WithMessage("Email must not exceed 25 characters.");

            RuleFor(f => f.Description).Length(0, 500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
