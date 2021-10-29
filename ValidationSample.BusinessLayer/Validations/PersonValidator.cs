using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using ValidationSample.BusinessLayer.Resources;
using ValidationSample.Shared.Models;

namespace ValidationSample.BusinessLayer.Validations
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator(IConfiguration configuration)
        {
            //CascadeMode = CascadeMode.Stop;

            RuleFor(p => p.FirstName).NotEmpty().WithMessage(Messages.FieldRequried)
                .MaximumLength(30)
                .WithName(Messages.FirstName);

            RuleFor(p => p.LastName).NotEmpty().WithMessage(Messages.FieldRequried)
                .MaximumLength(50)
                .WithName(Messages.LastName);

            RuleFor(p => p.BirthDate).LessThan(DateTime.UtcNow);

            RuleFor(p => p.Discount).NotEmpty().GreaterThan(0).When(p => p.HasDiscount);

            RuleFor(p => p.FiscalCode).Must(BeAValidFiscalCode).When(p => p.Country == "IT");

            RuleFor(p => p.Categories).NotEmpty().Must(MustBeUnique);
            RuleForEach(p => p.Categories).ChildRules(category =>
            {
                category.RuleFor(p => p.ProductsIds).NotEmpty();
            });

            RuleFor(p => p).Must((p, _) => BeAValidPerson(p));
        }

        private bool BeAValidPerson(Person person)
        {
            return true;
        }

        private bool MustBeUnique(IEnumerable<FavoriteCategory> categories)
        {
            var duplicates = categories.GroupBy(c => c.CategoryId)
                .Where(g => g.Count() > 1)
                .Select(k => k.Key);

            return !duplicates.Any();
        }

        private bool BeAValidFiscalCode(string fiscalCode)
        {
            return fiscalCode?.Length == 16;
        }
    }
}
