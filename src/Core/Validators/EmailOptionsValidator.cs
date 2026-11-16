using System;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Foundation.Core.Extensions;
using FluentValidation;

namespace Aurore.Archetypes.Api.Core.Validators;

public sealed class EmailOptionsValidator : AbstractValidator<EmailOptions>
{
    public EmailOptionsValidator(bool isProduction)
    {
        RuleFor(x => x.Host).NotEmpty();
        RuleFor(x => x.Port).InclusiveBetween(1, 65535);
        RuleFor(x => x.From).NotEmpty().EmailAddress();
        RuleForEach(x => x.ModeratorAddresses).NotEmpty().EmailAddress();
        RuleFor(x => x.ModeratorAddresses).NotEmpty().WithMessage("At least one moderator address is needed: it is who receives the reports.");

        RuleFor(x => x.Password).NotEmpty().When(x => x.Username.HasValue()).WithMessage("A Username needs a Password.");
        RuleFor(x => x.Username).NotEmpty().When(x => x.Password.HasValue()).WithMessage("A Password needs a Username.");

        When(_ => isProduction, () =>
        {
            RuleFor(x => x.EnableSsl).Equal(true).WithMessage("Production must send email over an encrypted connection.");
            RuleFor(x => x.Host)
                .Must(host => IsLocal(host) is false)
                .WithMessage("The Host is a local address; production must use a real mail server.");
        });
    }

    private static bool IsLocal(string host)
    {
        return host.IsEqualTo("localhost") || host.StartsWith("127.", StringComparison.Ordinal) || host.StartsWith("sbx-", StringComparison.OrdinalIgnoreCase);
    }
}
