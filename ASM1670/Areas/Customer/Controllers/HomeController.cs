using ASM1670.Data;
using ASM1670.Models;
using ASM1670.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace ASM1670.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _dbContext;
        private readonly int _recordsPerPage = 6;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index(int id, string searchString = "")
        {
            var products = _dbContext.Books
                .Where(b => b.Title.Contains(searchString))
                .Include(p => p.Category)
                .ToList();

            int numberOfRecords = products.Count();
            int numberOfPages = (int)Math.Ceiling((double)numberOfRecords / _recordsPerPage);

            ViewBag.numberOfPages = numberOfPages;
            ViewBag.currentPage = id;
            ViewData["Current Filter"] = searchString;

            var productList = products.Skip(id * numberOfPages).Take(_recordsPerPage).ToList();

            ViewData["Message"] = "Welcome!";
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // =================== Detail ==========================
        [HttpGet]
        public IActionResult Details(int id)
        {
            var productSelected = _dbContext.Books
                .Include(c => c.Category)
                .FirstOrDefault(b => b.Id == id);
            Cart cart = new Cart()
            {
                Book = productSelected,
                BookId = productSelected.Id
            };
            return View(cart);
        }

        // post
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public IActionResult Details(Cart cartObject)
        {
            cartObject.Id = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cartObject.UserId = claim.Value;

            if (cartObject.UserId == null || cartObject.BookId == null)
            {
                var productFromDb = _dbContext.Books
                    .Include(a => a.Category)
                    .FirstOrDefault(u => u.Id == cartObject.BookId);
                Cart cart = new Cart()
                {
                    Book = productFromDb,
                    BookId = productFromDb.Id
                };
                return View(cart);
            }


            Cart cartFromDb = _dbContext.Carts
                .FirstOrDefault(c => c.UserId == cartObject.UserId && c.BookId == cartObject.BookId);

            if (cartFromDb == null)
            {
                _dbContext.Add(cartObject);
                ViewData["Message"] = "Order successfully!";
            }
            else
            {
                cartFromDb.Count += cartObject.Count;
                _dbContext.Update(cartFromDb);

            }


            _dbContext.SaveChanges();

            // count product through session
            var count = _dbContext.Carts
                .Where(c => c.UserId == cartObject.UserId)
                .ToList().Count();

            if (HttpContext != null && HttpContext.Session != null)
            {
                HttpContext.Session.SetInt32(Constraintt.ssShoppingCart, count);
            }
            TempData["AddCartMessage"] = "You added an product into Cart!";
            TempData["ShowMessage"] = true; //Set flag to show message in the view
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> HelpScreen()
        {

            return View();
        }
    }
}
