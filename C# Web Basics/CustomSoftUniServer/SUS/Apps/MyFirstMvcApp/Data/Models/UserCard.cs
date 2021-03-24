namespace MyFirstMvcApp.Data.Models
{
    public class UserCard
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int CardId { get; set; }

        public virtual Card Card { get; set; }
    }
}