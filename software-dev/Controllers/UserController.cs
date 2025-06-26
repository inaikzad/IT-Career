
using Diary.Models.User;
using Diary.Services;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Controllers;

public class UserController : Controller
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(CreateUserDto userDto)
    {
        this.userService.Create(userDto);
        
        return RedirectToAction("Login", "User");
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginUserDto user)
    {
        this.userService.Login(user);
        
        return RedirectToAction("Index", "Home");
    }

    [HttpsPost]

    public IActionResult Logout()
    {
        this.userService.Logout(); 
        
        return RedirectToAction("Index", "Home");
    }
    
}

public class HttpsPostAttribute : Attribute
{
}