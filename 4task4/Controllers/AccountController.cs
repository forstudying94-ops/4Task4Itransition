using _4task4.DataBase;
using _4task4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _4task4.Controllers;

public class AccountController : Controller
{
    private readonly UserDBContext _context;

    public AccountController(UserDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
        {
            TempData["Error"] = "Email and password are required.";
            return View();
        }

        if (password.Length < 5)
        {
            TempData["Error"] = "Password must be at least 5 characters long.";
            return View();
        }

        var user = new UserDataModel();
        user.Id = Guid.NewGuid();
        user.Name = name ?? "";
        user.Email = email;
        user.Password = password;
        user.IsBlocked = false;
        user.EmailConfirmedStatus = false;
        user.RegisterTime = DateTime.UtcNow;
        user.LastRegisterTime = DateTime.UtcNow;

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "This email is already registered.";
            return View();
        }

        TempData["Success"] = "Registration successful. Please verify your email.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || user.Password != password)
        {
            TempData["Error"] = "Invalid email or password.";
            return View();
        }

        if (user.IsBlocked)
        {
            TempData["Error"] = "This account is blocked.";
            return View();
        }

        user.LastRegisterTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserName", string.IsNullOrWhiteSpace(user.Name) ? user.Email : user.Name);

        return RedirectToAction("Index", "UserCrud");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Verify()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Verify(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            TempData["Error"] = "User not found.";
            return View();
        }

        user.EmailConfirmedStatus = true;
        await _context.SaveChangesAsync();

        TempData["Success"] = "Your email has been confirmed.";
        return RedirectToAction("Index", "UserCrud");
    }
}
