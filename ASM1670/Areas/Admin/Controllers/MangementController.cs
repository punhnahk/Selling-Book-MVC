using System.Security.Claims;
using ASM1670.Data;
using ASM1670.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM1670.Controllers;

[Area(Constraintt.Admin)]
[Authorize(Roles = Constraintt.OwnerRole + "," + Constraintt.UserRole)]

public class ManagementController : Controller
{
    private readonly ApplicationDBContext _db;

    public ManagementController(ApplicationDBContext db)
    {
        _db = db;
    }
        
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        List<int> listId = new List<int>();

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        
        // check if user has the "storeowner" role
        bool isStoreOwner = User.IsInRole(Constraintt.OwnerRole);
        
        if (isStoreOwner)
        {
            // if user has "storeowner" role, display all orders
            var orderList = _db.Orders
                .Include(o => o.Users)
                .ToList();
            
            return View(orderList);
        }
        else
        {
            // if user has any other role, display only orders belonging to that user
            var orderList = _db.Orders
                .Where(x => x.UserId == claims.Value)
                .Include(o => o.Users)
                .ToList();
            
            return View(orderList);
        }
        

    }
        
    // detail of management
    [HttpGet]
    public IActionResult Detail(int managementId)
    {
        var managementDetail = _db.OrderDetails
            .Where(d => d.OrderId == managementId)
            .Include(d => d.Book)
            .ToList();


        return View(managementDetail);
    }
}