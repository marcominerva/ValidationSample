using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ValidationSample.Shared.Models;

namespace ValidationSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IValidator<Person> personValidator;

        public PeopleController(IValidator<Person> personValidator)
        {
            this.personValidator = personValidator;
        }

        [HttpPost]
        public IActionResult Save(Person person)
        {
            //var validationResults = personValidator.Validate(person);
            //var isValid = ModelState.IsValid;

            return NoContent();
        }
    }
}
