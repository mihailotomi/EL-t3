using FluentValidation;

namespace EL_t3.Core.Actions.Player.Queries.CheckConstraints;

public class PlayerClubConstraintValidator : AbstractValidator<PlayerClubConstraint>
{
    public PlayerClubConstraintValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id must not be empty and must be an integer.");
    }
}

public class PlayerCountryConstraintValidator : AbstractValidator<PlayerCountryConstraint>
{
    public PlayerCountryConstraintValidator()
    {
        RuleFor(x => x.CountryCode).NotEmpty().WithMessage("Country Code must not be empty.");
    }
}

public class PlayerAutocompleteQueryValidator : AbstractValidator<CheckPlayerConstraintsQuery>
{
    public PlayerAutocompleteQueryValidator()
    {
        RuleForEach(x => x.Constraints).SetInheritanceValidator(v =>
    {
        v.Add<PlayerClubConstraint>(new PlayerClubConstraintValidator());
        v.Add<PlayerCountryConstraint>(new PlayerCountryConstraintValidator());
    });
    }
}
