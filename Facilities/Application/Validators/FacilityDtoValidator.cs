using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Entities.Dtos;
using PhoneNumbers;

namespace Application.Validators
{
    public class FacilityDtoValidator : AbstractValidator<CreateFacilityDto>
    {
        public FacilityDtoValidator()
        {
            RuleFor(f => f.Name).NotEmpty().WithMessage("Facility name is required.")
                .MaximumLength(100).WithMessage("Facility name must not exceed 100 characters.");

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

                });

            RuleFor(f => f.WebsiteURL).NotEmpty().WithMessage("WebsiteUrl can't be empty.");


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
