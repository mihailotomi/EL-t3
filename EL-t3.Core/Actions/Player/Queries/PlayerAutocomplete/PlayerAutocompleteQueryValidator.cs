using FluentValidation;

namespace EL_t3.Core.Actions.Player.Queries.PlayerAutocomplete;

public class PlayerAutocompleteQueryValidator : AbstractValidator<PlayerAutocompleteQuery>
{
    private readonly int minSearchLength = 2;
    private readonly int maxSearchLength = 50;


    public PlayerAutocompleteQueryValidator()
    {
        RuleFor(x => x.Search)
            .NotEmpty().WithMessage("Search field must not be empty")
            .MinimumLength(minSearchLength)
            .WithMessage($"Search field must contain at least {minSearchLength} characters")
            .MaximumLength(maxSearchLength)
            .WithMessage($"Search field must not contain more than {maxSearchLength} characters");
    }
}