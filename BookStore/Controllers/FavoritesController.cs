
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BookStore.Data;
using BookStore.Models;
namespace BookStore.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly UserManager<DefaultUser> _userManager;

        public FavoritesController(BookStoreContext context, UserManager<DefaultUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Add a book to favorites
        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int bookId)
        {
            var userId = _userManager.GetUserId(User);
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.BookId == bookId && f.UserId == userId);

            if (existingFavorite == null)
            {
                var favorite = new Favorite
                {
                    UserId = userId,
                    BookId = bookId
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Store"); // Redirect to your store or favorite view
        }

        // Remove a book from favorites
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int favoriteId)
        {
            var favorite = await _context.Favorites.FindAsync(favoriteId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Store"); // Redirect back to the store or favorite view
        }

        // View the user's favorite books
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Book) // To include related books
                .ToListAsync();

            return View(favorites);
        }
    }
}
