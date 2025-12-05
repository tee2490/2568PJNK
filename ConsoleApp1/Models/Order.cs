namespace ConsoleApp1.Models
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Qty { get; set; }
    }

    public class Order
    {
        public int OrderNo { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

}
