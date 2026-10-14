using Clinic_BusinessLayer.DTOs.Requests.Payments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic_BusinessLayer.Validators.Payments
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentRequest>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.InvoiceId)
                .GreaterThan(0).WithMessage("Invoice is required");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(x => x.Method)
                .NotEmpty().WithMessage("Payment method is required")
                .Must(m => new[] { "Cash", "Card", "Insurance" }.Contains(m))
                .WithMessage("Invalid payment method. Allowed: Cash, Card, Insurance");
        }
    }
}
