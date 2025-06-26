using Microsoft.AspNetCore.Identity;

namespace Diary.Services;

public class PasswordHasherService
{
    private PasswordHasher<string> _hasher = new PasswordHasher<string>();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null, password);
    }

    public bool VerifyPassword(string hashedPassword, string input)
    {
        var result = _hasher.VerifyHashedPassword(null, hashedPassword, input);
        return result == PasswordVerificationResult.Success;
    } 
}