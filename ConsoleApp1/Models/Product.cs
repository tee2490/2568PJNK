namespace ConsoleApp1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }


        //construtor
        public Product(int id, Category category, double price)
        {
            Id = id;
            Name = $"PRODUCT{id}";
            Category = category;
            Price = price;
        }
    }

    public enum Category
    {
        Drink,
        Food,
        General,
        Cloth
    }

}
