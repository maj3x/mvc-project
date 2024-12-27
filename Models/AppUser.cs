// Models/AppUser.cs

using System;
using Microsoft.AspNetCore.Identity;
public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedDate { get; set; }
}

// Models/AppRole.cs
public class AppRole : IdentityRole
{
    public DateTime CreatedDate { get; set; }
}
