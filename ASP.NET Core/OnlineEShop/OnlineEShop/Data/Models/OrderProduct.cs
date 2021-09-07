namespace OnlineEShop.Data.Models
{

    public class OrderProduct 
    {
        public int Id { get; init; }
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
