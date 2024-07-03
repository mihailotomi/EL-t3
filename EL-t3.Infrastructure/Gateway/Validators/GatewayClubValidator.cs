using EL_t3.Infrastructure.Gateway.Contracts;
using FluentValidation;

namespace EL_t3.Infrastructure.Gateway.Validators;

public class GatewayClubValidator : AbstractValidator<GatewayClub>
{
    public GatewayClubValidator()
    {
        RuleFor(gc => gc.Code).NotEmpty();
        RuleFor(gc => gc.Name).NotEmpty();
    }
}