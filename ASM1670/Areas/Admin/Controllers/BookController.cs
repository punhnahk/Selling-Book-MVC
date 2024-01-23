using System.Security.Claims;
using ASM1670.Data;
using ASM1670.Models;
using ASM1670.Utility;
using ASM1670.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASM1670.Controllers;

[Area("Admin")]
[Authorize(Roles = "Owner")]

public class BookController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly IWebHostEnvironment _environment;

    public BookController(ApplicationDBContext context,  IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
    
     // GET
        // Index
        [HttpGet]
        public IActionResult Index()
        {
            // lay id cua user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var currentUserId = claim.Value;
            
            var books = _context.Books
                .Where(x => x.CreateBy == currentUserId)
                .Include(x => x.Category).ToList();
            return View(books);
        }
        // Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var objBook = _context.Books.Find(id);
            _context.Books.Remove(objBook);
            _context.SaveChanges();

            TempData["DeleteBoMessage"] = "Deleted Book Successfully!";
            TempData["ShowMessage"] = true; //Set flag to show message in the view
            return RedirectToAction(nameof(Index));
        }
    // CreateUpdate
    [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            var bookVm = new BookVm();

            bookVm.Categories = CategorySelectListItems();

            
            if (id == null)
            {
                bookVm.Book = new Book();
                return View(bookVm);
            }
            var book = _context.Books.Find(id);
            bookVm.Book = book;
            return View(bookVm);
        }
        [HttpPost]
        public IActionResult CreateUpdate(BookVm bookVm)
        {
            if (ModelState.IsValid)
            {
                bookVm.Categories = CategorySelectListItems();
                
                return View(bookVm);
            }

            var webRootPath = _environment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var fileName = Guid.NewGuid();
                var uploads = Path.Combine(webRootPath, @"img/books");
                var extension = Path.GetExtension(files[0].FileName);
                if (bookVm.Book.Id != 0)
                {
                    var productDb = _context.Books.AsNoTracking()
                        .Where(b => b.Id == bookVm.Book.Id).First();
                    if (productDb.ImageUrl != null && bookVm.Book.Id != 0)
                    {
                        var imagePath = Path.Combine(webRootPath, productDb.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                    }
                }

                using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStreams);
                }

                bookVm.Book.ImageUrl = @"/img/books/" + fileName + extension;
            }

            else
            {
                bookVm.Categories = CategorySelectListItems();
                return View(bookVm);
            }


            if (bookVm.Book.Id == 0 || bookVm.Book.Id == null)
            {
                // lay id cua user
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var currentUserId = claim.Value;
                bookVm.Book.CreateBy = currentUserId;
                
                _context.Books.Add(bookVm.Book);
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var currentUserId = claim.Value;
                bookVm.Book.CreateBy = currentUserId;
                _context.Books.Update(bookVm.Book);
            }
            _context.SaveChanges();
            TempData["CreateBoMessage"] = "Created Book Successfully!";
            TempData["ShowMessage"] = true; //Set flag to show message in the view
            return RedirectToAction(nameof(Index));

        }

        // method for category select list VM
        private IEnumerable<SelectListItem> CategorySelectListItems()
        {
            var categories = _context.Categories
                .Where(c => c.Status == Category.StatusCategory.Approve)
                .ToList();

            // for each book
            var result = categories
                .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return result;
        }       
}