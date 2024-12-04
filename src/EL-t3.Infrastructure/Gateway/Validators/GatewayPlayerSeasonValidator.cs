using EL_t3.Infrastructure.Gateway.Contracts;
using FluentValidation;

namespace EL_t3.Infrastructure.Gateway.Validators;

public class GatewayCountryValidator : AbstractValidator<GatewayCountry>
{
    public GatewayCountryValidator()
    {
        RuleFor(c => c.Code).NotEmpty();
    }
}

public class GatewayPersonValidator : AbstractValidator<GatewayPerson>
{
    public GatewayPersonValidator()
    {
        RuleFor(p => p.Name).NotEmpty().Must(name => name.Contains(','));
        RuleFor(p => p.Country).NotEmpty().SetValidator(new GatewayCountryValidator());
        RuleFor(p => p.BirthDate).NotEmpty();
    }
}

public class GatewaySeasonValidator : AbstractValidator<GatewaySeason>
{
    public GatewaySeasonValidator()
    {
        RuleFor(s => s.Year).NotEmpty();
    }
}

public class GatewayPlayerSeasonValidator : AbstractValidator<GatewayPlayerSeason>
{
    public GatewayPlayerSeasonValidator()
    {
        RuleFor(ps => ps.Person).NotEmpty().SetValidator(new GatewayPersonValidator());
        RuleFor(ps => ps.StartDate).NotNull();
        RuleFor(ps => ps.EndDate).NotNull();
        RuleFor(ps => ps.Club).NotEmpty().SetValidator(new GatewayClubValidator());
        RuleFor(ps => ps.Season).NotEmpty().SetValidator(new GatewaySeasonValidator());
    }
}