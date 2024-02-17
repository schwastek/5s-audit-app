using Api.Contracts.Identity.Requests;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace Core.Identity;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.DisplayName)
            .NotEmpty();

        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(r => r.Username)
            .NotEmpty();

        RuleFor(r => r.Password)
            .NotEmpty()
            .Must(PasswordMatchesStrengthCriteria)
            .WithMessage("Password is too weak. Password must be between 4 and 8 characters long, " +
                "including one lowercase letter, one uppercase letter, one number.");
    }

    private bool PasswordMatchesStrengthCriteria(string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        // Password strength requirements:
        //
        // * At least one digit (\d)
        // * At least one lowercase letter ([a-z])
        // * At least one uppercase letter ([A-Z])
        // * The length of the password must be between 4 and 8 characters
        const string pattern = "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$";

        // Check
        var rg = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(2.0));
        var result = rg.IsMatch(password);

        return result;
    }
}
