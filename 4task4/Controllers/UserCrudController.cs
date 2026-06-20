using _4task4.DataBase;
using _4task4.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _4task4.Controllers;

[AuthFilter]
public class UserCrudController : Controller
{
    private readonly UserDBContext _context;

    public UserCrudController(UserDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.OrderByDescending(u => u.LastRegisterTime).ToListAsync();
        return View(users);
    }

    [HttpPost]
    [SelectionRequiredFilter]
    public async Task<IActionResult> UserBlock(List<Guid> selectedIds)
    {
        var users = await _context.Users.Where(u => selectedIds.Contains(u.Id)).ToListAsync();
        foreach (var user in users)
        {
            user.IsBlocked = true;
        }
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [SelectionRequiredFilter]
    public async Task<IActionResult> UserUnblock(List<Guid> selectedIds)
    {
        var users = await _context.Users.Where(u => selectedIds.Contains(u.Id)).ToListAsync();
        foreach (var user in users)
        {
            if (user.IsBlocked)
            {
                user.IsBlocked = false;
                user.EmailConfirmedStatus = false;
            }
        }
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [SelectionRequiredFilter]
    public async Task<IActionResult> UserDelete(List<Guid> selectedIds)
    {
        var users = await _context.Users.Where(u => selectedIds.Contains(u.Id)).ToListAsync();
        _context.Users.RemoveRange(users);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUnverified()
    {
        var users = await _context.Users.Where(u => !u.EmailConfirmedStatus).ToListAsync();
        _context.Users.RemoveRange(users);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
