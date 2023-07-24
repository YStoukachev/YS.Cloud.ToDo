using FluentValidation;
using FluentValidation.Results;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Validators
{
    public class ToDoItemValidator : AbstractValidator<ToDoItemModel>
    {
        public ToDoItemValidator()
        {
            RuleFor(_ => _.Title).NotEmpty();
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<ToDoItemModel> context,
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