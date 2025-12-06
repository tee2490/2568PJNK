using ConsoleApp3.Models;

namespace ConsoleApp3.Services
{
    public class GenData
    {
        Random rand = new Random();
        public static List<Product> products = new List<Product>();

        public void Create()
        {
            var N = rand.Next(10, 16);
            products = new();

            for (int i = 1; i <= N; i++)
            {
                Category category = (Category)rand.Next(Enum.GetNames(typeof(Category)).Length);
                double price = rand.Next(10, 31) + rand.NextDouble();
                price = Math.Round(price,2);

                products.Add(new Product(i, category, price));
            }

        }

        public void Show(List<Product> pd)
        {
            string[] h = ["ID","Name","Category", "Price"];
            Console.WriteLine($"{h[0],-5} {h[1],-15} {h[2],-10} {h[3],10}");
            foreach (var p in pd)
            {
                Console.WriteLine($"{p.Id,-5} {p.Name,-15} {p.Category,-10} {p.Price,10}");
            }
        }

        public void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("=== เพิ่มสินค้า ===");

            Product p = new Product();

            if (products.Count() > 0 )
            p.Id = products.Max(p => p.Id)+1;
            else p.Id = 1;

            Console.WriteLine($"ID: {p.Id}");
            
            Console.Write("Name: ");
            p.Name = Console.ReadLine();

            Console.WriteLine("Category (0=Drink, 1=Food, 2=General, 3=Cloth)");
            Console.Write("เลือกหมวด: ");
            int catChoice = int.TryParse(Console.ReadLine(), out int c) && c is >= 0 and <= 3 ? c : 0;
            p.Category = (Category)catChoice;

            Console.Write("Price: ");
            p.Price = double.Parse(Console.ReadLine());
            p.Price = Math.Round(p.Price,2);

            products.Add(p);

            Console.WriteLine("เพิ่มข้อมูลเรียบร้อย\n");
            Show(products);
        }




    }
}
