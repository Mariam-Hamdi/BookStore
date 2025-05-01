using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // Assuming you use ASP.NET Identity for users

        [Required]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}
