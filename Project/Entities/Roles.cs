using Microsoft.AspNetCore.Identity;

namespace Project.Entities;

public class Roles : IdentityRole
{
    public static readonly string ADMIN = "admin";
    public static readonly string USER = "user";
}
