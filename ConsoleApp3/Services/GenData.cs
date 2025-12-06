using ConsoleApp3.Models;

namespace ConsoleApp3.Services
{
    public class GenData
    {
        Random rand = new Random();
        public static List<Product> products = new List<Product>();

        public void Create()
        {
            var N = rand.Next(10, 30);
            products = new();

            for (int i = 1; i <= N; i++)
            {
                Category category = (Category)rand.Next(Enum.GetNames(typeof(Category)).Length);
                double price = rand.Next(10, 31) + rand.NextDouble();
                price = Math.Round(price,1);

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

        public void EditProduct()
        {
            Console.Clear();
            Console.WriteLine("=== แก้ไขสินค้า ===");
            Show(products);

            Console.Write("กรอก ID ที่ต้องการแก้ไข: ");
            int id = int.Parse(Console.ReadLine());

            // หา Product
            var p = products.FirstOrDefault(x => x.Id == id);

            if (p == null)
            {
                Console.WriteLine("ไม่พบสินค้า");
                return;
            }

            Console.WriteLine($"สินค้าเดิม: {p.Id} {p.Name} {p.Category} {p.Price}");

            Console.Write("Name: ");
            p.Name = Console.ReadLine();

            Console.WriteLine("Category (0=Drink, 1=Food, 2=General, 3=Cloth)");
            Console.Write("เลือกหมวด: ");
            int catChoice = int.TryParse(Console.ReadLine(), out int c) && c is >= 0 and <= 3 ? c : 0;
            p.Category = (Category)catChoice;

            Console.Write("Price: ");
            p.Price = Math.Round(double.Parse(Console.ReadLine()), 2);

            Console.WriteLine("\nแก้ไขข้อมูลเรียบร้อย");
            Show(products);
        }

        public void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("=== ลบสินค้า ===");
            Show(products);

            Console.Write("กรอก ID ที่ต้องการลบ: ");
            int id = int.Parse(Console.ReadLine());

            var p = products.FirstOrDefault(x => x.Id == id);

            if (p == null)
            {
                Console.WriteLine("ไม่พบสินค้า");
                return;
            }

            products.Remove(p);

            Console.WriteLine("ลบข้อมูลเรียบร้อย\n");
            Show(products);
        }

        public void SortProduct()
        {
            Console.Clear();
            Console.WriteLine("=== เมนูจัดการข้อมูลสินค้า ===");
            Console.WriteLine("1. เรียงราคา (มาก → น้อย)");
            Console.WriteLine("2. จัดกลุ่มตาม Category");
            Console.WriteLine("3. สินค้าที่มีราคาสูงที่สุด");
            Console.WriteLine("4. แสดงรายการสินค้าที่มีราคาสูงที่สุด 3 ราคา");
            Console.WriteLine("5. แสดงรายการสินค้าราคา 3 ช่วง 10-15.99 | 16-20.99 | >17");
            Console.Write("เลือกเมนู (1-5): ");

            int choice = int.TryParse(Console.ReadLine(), out int c) ? c : 0;

            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    SortByPriceDesc();
                    break;

                case 2:
                    GroupByCategory();
                    break;

                case 3:
                    ShowMaxPriceProduct();
                    break;

                case 4:
                    ShowProductTop3Prices();
                    break;

                case 5:
                    ShowProduct3Ranges();
                    break;

                default:
                    Console.WriteLine("เลือกเมนูไม่ถูกต้อง");
                    break;
            }

            Console.WriteLine("\nกด Enter เพื่อกลับเมนู...");
            Console.ReadLine();
        }


        private void SortByPriceDesc()
        {
            Console.WriteLine("=== เรียงราคา (มาก → น้อย) ===\n");

            var result = products
                            .OrderByDescending(p => p.Price)
                            .ToList();

            Show(result);
        }


        private void GroupByCategory()
        {
            Console.WriteLine("=== จัดกลุ่มตาม Category ===\n");

            var groups = products.GroupBy(p => p.Category)
                                 .OrderBy(g => g.Key);

            foreach (var g in groups)
            {
                Console.WriteLine($">>> {g.Key} ({g.Count()} ชิ้น)");
                Show(g.ToList());
                Console.WriteLine();
            }
        }

        private void ShowMaxPriceProduct()
        {
            Console.WriteLine("=== สินค้าที่มีราคาสูงที่สุด ===\n");

            double maxPrice = products.Max(p => p.Price);

            var result = products
                            .Where(p => p.Price == maxPrice)
                            .OrderBy(p => p.Id)
                            .ToList();

            Show(result);
        }

        private void ShowProductTop3Prices()
        {
            Console.WriteLine("=== TOP 3 ราคาแพงที่สุด (แบบจัดกลุ่ม) ===\n");

            var groups = products
                            .GroupBy(p => p.Price)
                            .OrderByDescending(g => g.Key)
                            .Take(3)
                            .ToList();

            foreach (var g in groups)
            {
                Console.WriteLine($">>> ราคา {g.Key:F2} (มี {g.Count()} รายการ)");
                Show(g.ToList());
                Console.WriteLine();
            }
        }

        private void ShowProduct3Ranges()
        {
            Console.WriteLine("=== แสดงรายการสินค้าตามช่วงราคา ===\n");

            // ช่วงที่ 1 : 10 - 15.99
            Console.WriteLine(">>> ช่วงราคา 10 - 15.99");
            var r1 = products
                        .Where(p => p.Price >= 10 && p.Price <= 15.99)
                        .OrderBy(p => p.Price)
                        .ToList();
            Show(r1);
            Console.WriteLine();

            // ช่วงที่ 2 : 16 - 16.99
            Console.WriteLine(">>> ช่วงราคา 16 - 20.99");
            var r2 = products
                        .Where(p => p.Price >= 16 && p.Price <= 20.99)
                        .OrderBy(p => p.Price)
                        .ToList();
            Show(r2);
            Console.WriteLine();

            // ช่วงที่ 3 : ราคา > 17
            Console.WriteLine(">>> ช่วงราคา > 17");
            var r3 = products
                        .Where(p => p.Price > 17)
                        .OrderBy(p => p.Price)
                        .ToList();
            Show(r3);
            Console.WriteLine();
        }



    }
}
