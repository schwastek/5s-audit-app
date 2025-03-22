using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Accounts.BusinessRules;

public interface IAccountBusinessRules
{
    public Task<bool> IsEmailAvailable(string? email, CancellationToken cancellationToken);
    public Task<bool> IsUsernameAvailable(string? username, CancellationToken cancellationToken);
    public bool PasswordMatchesStrengthCriteria(string? password);
}

public class AccountBusinessRules : IAccountBusinessRules
{
    private readonly UserManager<User> _userManager;

    public AccountBusinessRules(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> IsEmailAvailable(string? email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email)) return false;

        if (await _userManager.Users.AnyAsync(x => x.Email == email, cancellationToken))
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsUsernameAvailable(string? username, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(username)) return false;

        if (await _userManager.Users.AnyAsync(x => x.UserName == username, cancellationToken))
        {
            return false;
        }

        return true;
    }

    public bool PasswordMatchesStrengthCriteria(string? password)
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
