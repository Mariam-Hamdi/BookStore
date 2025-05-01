namespace BookStore.Models
{
    public class FavoriteItem
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public string FavoriteId { get; set; }
    }
}
