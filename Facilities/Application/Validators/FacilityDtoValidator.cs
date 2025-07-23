using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Application.Dtos;
using PhoneNumbers;

namespace Application.Validators
{
    public class FacilityDtoValidator : AbstractValidator<FacilityDto>
    {
        public FacilityDtoValidator()
        {
            RuleFor(f => f.Name).NotEmpty().WithMessage("Facility name is required.")
                .MaximumLength(100).WithMessage("Facility name must not exceed 100 characters.");

            RuleFor(f => f.Address).NotEmpty().WithMessage("Facility address is required.");

            RuleFor(f => f.PhoneNumber).Must(ValidPhoneNumber).WithMessage("Phone number must be in a valid international format.");

            RuleFor(f => f.Schedule).NotEmpty().WithMessage("Schedule can't be empty");

            RuleFor(f => f.Email).EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(25).WithMessage("Email must not exceed 25 characters.");

            RuleFor(f => f.Description).Length(0, 500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(f => f.Schedule).NotEmpty().WithMessage("Schedule can't be empty.");

            RuleForEach(f => f.Schedule)
                .ChildRules(schedule =>
                {
                    schedule.RuleFor(e => e.Key)
                        .IsInEnum().WithMessage("Schedule day must be a valid day of the week.");
                    schedule.RuleFor(e => e.Value)
                        .Matches(@"^([01][0-9]|2[0-3]):[0-5][0-9]-([01][0-9]|2[0-3]):[0-5][0-9]$")
                        .WithMessage("Schedule time must be in hh:mm-hh:mm format.");
                });

            RuleFor(f => f.WebsiteURL).NotEmpty().WithMessage("WebsiteUrl can't be empty.");

            RuleFor(f => f.Longitude).InclusiveBetween(-180, 180).WithMessage("Latitude must be between -180 and 180 degrees."); ;
            RuleFor(f => f.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90 degrees.");
        }

        private bool ValidPhoneNumber(string? phoneNumber)
        {
            try
            {
                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                var number = phoneNumberUtil.Parse(phoneNumber, null);
                return phoneNumberUtil.IsValidNumber(number);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
