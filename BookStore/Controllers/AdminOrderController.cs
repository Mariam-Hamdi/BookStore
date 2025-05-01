using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly BookStoreContext _context;

        public AdminOrderController(BookStoreContext context)
        {
            _context = context;
        }

        // أكشن لعرض جميع الطلبات
        public IActionResult OrdersList()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .ToList();

            return View(orders);
        }

    }
}
