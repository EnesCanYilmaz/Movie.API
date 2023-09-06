using System;
using Microsoft.AspNetCore.Identity;

namespace MovieAPI.Infrastructure.Data.Entities.App;

public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; }
    public DateTime CreatedDate { get; set; }
}

