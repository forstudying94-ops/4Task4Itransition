using System.ComponentModel.DataAnnotations;

namespace _4task4.Models;

public class UserDataModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool IsBlocked { get; set; } = false;

    public bool EmailConfirmedStatus { get; set; } = false;

    public DateTime RegisterTime { get; set; } = DateTime.UtcNow;

    public DateTime LastRegisterTime { get; set; } = DateTime.UtcNow;
}
