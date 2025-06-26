using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Diary.Data;
using Diary.Models;
using Diary.Models.Entities;
using Diary.Models.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;

namespace Diary.Services;

public class UserService : IUserService 
{
    private readonly ApplicationDbContext dbContext;
    private readonly PasswordHasherService passwordHasherService;
    private readonly LoggedUserService loggedUserService;

    public UserService(ApplicationDbContext dbContext, PasswordHasherService passwordHasherService, 
        LoggedUserService loggedUserService)
    {
    
        this.dbContext = dbContext;
        this.passwordHasherService = passwordHasherService;
        this.loggedUserService = loggedUserService;
    }
    
    public void Create(CreateUserDto user)
    {
        User newUser = ToUser(user);
        newUser.Password = this.passwordHasherService.HashPassword(user.Password);
        dbContext.Users.Add(newUser);
        dbContext.SaveChanges();
    }
    
    public void Login(LoginUserDto user)
    {
        User dbUser = dbContext.Users
            .FirstOrDefault(u => u.Username == user.Username);

        if (dbUser == null)
        {
            throw new Exception("Invalid username or password");
        }
        
       bool isValid = this.passwordHasherService.VerifyPassword(dbUser.Password, user.Password);
        
        if (isValid)
        {
            throw new Exception("Invalid username or password");
        }

        this.loggedUserService.User = dbUser;
    }
    
    public void Logout()
    {
        this.loggedUserService.User = null;
    }
    
    private User ToUser(CreateUserDto user)
    {
        User u = new User();
        u.Username = user.Username; 
        u.FirstName = user.FirstName;
        u.LastName =  user.LastName;
        u.Email = user.Email;
        return u;
    }

    // Algorith from Internet 
    private string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
        
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        
    }
}