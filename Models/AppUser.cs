using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
}