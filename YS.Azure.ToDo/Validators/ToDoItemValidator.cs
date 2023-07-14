using FluentValidation;
using FluentValidation.Results;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Validators
{
    public class ToDoItemValidator : AbstractValidator<ToDoItem>
    {
        public ToDoItemValidator()
        {
            RuleFor(_ => _.Title).NotEmpty();
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<ToDoItem> context,
            CancellationToken cancellation = new CancellationToken())
        {
            if (context.InstanceToValidate == null)
            {
                return new ValidationResult(new List<ValidationFailure>()
                {
                    new ValidationFailure()
                });
            }

            return await base.ValidateAsync(context, cancellation);
        }
    }
}