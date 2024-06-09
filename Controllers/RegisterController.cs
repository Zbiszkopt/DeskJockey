using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DeskJockey.Models;
using DeskJockey.Data;
using Microsoft.EntityFrameworkCore;

namespace DeskJockey.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Register
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User user)
        {
            if (ModelState.IsValid)
            {
                // Sprawdź, czy użytkownik o podanym adresie email już istnieje w bazie danych
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser == null)
                {
                    // Dodaj nowego użytkownika do bazy danych
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Rejestracja zakończona pomyślnie. Teraz możesz się zalogować.";
                    // Przenieś użytkownika do strony logowania po pomyślnej rejestracji
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Użytkownik o podanym adresie email już istnieje.");
                    return View(user);
                }
            }
            return View(user);
        }
    }
}