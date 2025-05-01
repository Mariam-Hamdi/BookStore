using BookStore.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class Cart
    {
        private readonly BookStoreContext _context;

       
        public string Id { get; set; }
        public List<CartItem> CartItems { get; set; }


      

    }
}
