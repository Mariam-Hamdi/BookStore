using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(BookStoreContext context, IHttpContextAccessor httpContextAccessor)
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

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            string cartId = GetCartId();

            var cartItems = _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Book)
                .ToList();

            if (cartItems.Count == 0)
            {
                ModelState.AddModelError("", "Cart is empty, please add a book first.");
            }

            if (ModelState.IsValid)
            {
                order.OrderPlaced = DateTime.Now;
                order.OrderItems = new List<OrderItem>();
                order.OrderTotal = 0;

                foreach (var item in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        Quantity = item.Quantity,
                        BookId = item.Book.Id,
                        Price = item.Book.Price * item.Quantity
                    };

                    order.OrderItems.Add(orderItem);
                    order.OrderTotal += orderItem.Price;
                }

                _context.Orders.Add(order);
                _context.SaveChanges();

                // Clear the cart after checkout
                _context.CartItems.RemoveRange(cartItems);
                _context.SaveChanges();

                return View("CheckoutComplete", order);
            }

            return View(order);
        }

        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }
    }
}
