using ASM1670.Constants;
using ASM1670.Data;
using ASM1670.Models;
using ASM1670.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASM1670.AutoCreateRole;

public class AutoCreateRole : IAutoCreateRole
{
    private readonly ApplicationDBContext _db;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AutoCreateRole(ApplicationDBContext db, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void CreateRole()
    {
        if (_db.Roles.Any(r => r.Name == Constraintt.AdminRole)) return;
        if (_db.Roles.Any(r => r.Name == Constraintt.OwnerRole)) return;
        if (_db.Roles.Any(r => r.Name == Constraintt.UserRole)) return;

        // this will deploy if there no have any role yet ( add cai role vao role manger)
        _roleManager.CreateAsync(new IdentityRole(Constraintt.AdminRole)).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole(Constraintt.OwnerRole)).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole(Constraintt.UserRole)).GetAwaiter().GetResult();

        // create user admin ( cai nay no se tao san mot thang user admin moi khi ma ung dung khoi chay)
        _userManager.CreateAsync(new User()
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            FullName = "Admin",
            PhoneNumber = "1234566",
            HomeAddress = "Admin123",
            EmailConfirmed = true,
        }, "Admin@123").GetAwaiter().GetResult();


        // finding the user which is just have created (tao admin object)
        var admin = _db.Users.FirstOrDefault(a => a.Email == "admin@gmail.com");

        // add that user (admin) to admin role ( sau do add role cho admin roi tao ra no)
        _userManager.AddToRoleAsync(admin, Constraintt.AdminRole).GetAwaiter().GetResult();
    }
}