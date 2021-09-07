namespace OnlineEShop.Data.Models
{
    public class Vote
    {
        public int Id { get; init; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public byte Value { get; set; }
    }
}
