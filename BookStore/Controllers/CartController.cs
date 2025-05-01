using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BookStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(BookStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCartId()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string cartId = session.GetString("Id") ?? Guid.NewGuid().ToString();
            session.SetString("Id", cartId);
            return cartId;
        }

        public IActionResult Index()
        {
            string cartId = GetCartId();

            var items = _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Book)
                .ToList();

            var cart = new Cart
            {
                Id = cartId,
                CartItems = items
            };

            ViewBag.Total = items.Sum(ci => ci.Book.Price * ci.Quantity);
            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();

            string cartId = GetCartId();
            var cartItem = _context.CartItems
                .SingleOrDefault(ci => ci.Book.Id == book.Id && ci.CartId == cartId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Book = book,
                    Quantity = 1,
                    CartId = cartId
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Store");
        }

        public IActionResult RemoveFromCart(int id)
        {
            string cartId = GetCartId();

            var cartItem = _context.CartItems
                .SingleOrDefault(ci => ci.Book.Id == id && ci.CartId == cartId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ReduceQuantity(int id)
        {
            string cartId = GetCartId();

            var cartItem = _context.CartItems
                .SingleOrDefault(ci => ci.Book.Id == id && ci.CartId == cartId);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(int id)
        {
            string cartId = GetCartId();

            var cartItem = _context.CartItems
                .SingleOrDefault(ci => ci.Book.Id == id && ci.CartId == cartId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            string cartId = GetCartId();

            var cartItems = _context.CartItems
                .Where(ci => ci.CartId == cartId);

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
