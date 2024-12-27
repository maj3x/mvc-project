using System;
using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Models
{
    public class AppRole : IdentityRole
    {
        public DateTime CreatedDate { get; set; }
    }
}
