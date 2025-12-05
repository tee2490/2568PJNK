
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class GenData
    {
        Random rand = new Random();
        public static List<Product> products = new List<Product>();

        public static List<Order> orders = new List<Order>();

        public GenData()
        {
            Create();
        }

        public void Create()
        {
            var N = rand.Next(10, 16);

            for (int i = 1; i <= N; i++)
            {
                Category category = (Category)rand.Next(Enum.GetNames(typeof(Category)).Length);
                double price = rand.Next(10, 31) + rand.NextDouble();

                products.Add(new Product(i, category, price));
            }

        }

        public void Show()
        {
            Console.WriteLine("ID\tName\t\tCategory\tPrice");
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id}\t{p.Name}\t{p.Category}\t\t{p.Price:N2}");
            }
        }

        public void ShowMaxPrice()
        {
            double max = products.Max(p => p.Price);
            Console.WriteLine("\nMax Price of Products");
            Console.WriteLine("ID\tName\t\tCategory\tPrice");
            foreach (var p in products.Where(p => p.Price == max).OrderBy(p => p.Id))
                Console.WriteLine($"{p.Id}\t{p.Name}\t{p.Category}\t\t{p.Price:N2}");
        }

        public void ShowMinPrice()
        {
            double min = products.Min(p => p.Price);
            Console.WriteLine("\nMin Price of Product");
            Console.WriteLine("ID\tName\t\tCategory\tPrice");
            foreach (var p in products.Where(p => p.Price == min).OrderBy(p => p.Id))
                Console.WriteLine($"{p.Id}\t{p.Name}\t{p.Category}\t\t{p.Price:N2}");
        }

        public void ShowByCategory()
        {
            var groups = products
        .OrderBy(p => p.Category)
        .ThenBy(p => p.Id)
        .GroupBy(p => p.Category);

            foreach (var g in groups)
            {
                Console.WriteLine($"\n{g.Key} = {g.Count()}");
                Console.WriteLine("ID\tName\t\tCategory\tPrice");

                foreach (var p in g)
                    Console.WriteLine($"{p.Id}\t{p.Name}\t{p.Category}\t\t{p.Price:N2}");
            }
        }


        //Part2
        public void CreateOrders()
        {
            orders = new List<Order>();

            Console.Write("Number of Customer: ");
            int A = int.Parse(Console.ReadLine());

            Console.Write("Maximum of Product: ");
            int M = int.Parse(Console.ReadLine());

            for (int no = 1; no <= A; no++)
            {
                var order = new Order { OrderNo = no };

                int count = rand.Next(1, M + 1);

                // สุ่มสินค้าแต่ละชิ้น (ให้มีสิทธิ์ซ้ำกันได้)
                for (int i = 0; i < count; i++)
                {
                    var p = GenData.products[rand.Next(GenData.products.Count)];
                    order.Items.Add(new OrderItem { Product = p, Qty = 1 });
                }

                orders.Add(order);
                ShowOrder(order);
            }
        }

        private void ShowOrder(Order order)
        {
            Console.WriteLine($"\nOrder No.{order.OrderNo}:");
            Console.WriteLine();
            Console.WriteLine("ID\tNAME\t\tCATE\tPrice\tQty");

            double sum = 0;
            double discount = 0;

            // 1) แสดงสินค้า “ซ้ำได้”
            foreach (var item in order.Items.OrderBy(x => x.Product.Id))
            {
                var p = item.Product;
                sum += p.Price * item.Qty;

                Console.WriteLine($"{p.Id}\t{p.Name}\t{p.Category}\t{p.Price:N2}\t{item.Qty}");
            }

            // 2) คำนวณส่วนลดด้วย GroupBy “เฉพาะตอนคิดส่วนลด”
            var discountRows = order.Items
                .GroupBy(i => i.Product.Id)
                .Select(g => new
                {
                    Product = g.First().Product,
                    Qty = g.Sum(x => x.Qty)
                });

            foreach (var row in discountRows)
            {
                int pair = row.Qty / 2; // ซื้อเป็นคู่
                if (pair > 0)
                {
                    double pairTotal = row.Product.Price * 2 * pair;
                    discount += pairTotal * 0.10;
                }
            }

            double net = sum - discount;

            Console.WriteLine($"\nSum = {sum:N2}");
            Console.WriteLine($"Discount = {discount:N2}");
            Console.WriteLine($"Net = {net:N2}");
            Console.WriteLine("-----------------------------------");
        }

        //Part3
        public void ShowPart3()
        {
            if (orders == null || orders.Count == 0)
            {
                Console.WriteLine("ยังไม่มีข้อมูลคำสั่งซื้อ (เรียก CreateOrders3() ก่อน)");
                return;
            }

            // รวมสินค้าทุกออเดอร์
            var allItems = orders.SelectMany(o => o.Items).ToList();

            //
            // 1) สินค้าที่ขายดีที่สุด (รวมจำนวน Qty)
            //
            var bestSell = allItems
                .GroupBy(i => i.Product.Id)
                .Select(g => new
                {
                    Product = g.First().Product,
                    Num = g.Sum(x => x.Qty)
                })
                .OrderByDescending(x => x.Num)
                .First();

            Console.WriteLine("\n1. The best seller and number");
            Console.WriteLine("ID\tNAME\t\tCATE\tPrice\tNum");
            Console.WriteLine($"{bestSell.Product.Id}\t{bestSell.Product.Name}\t{bestSell.Product.Category}\t{bestSell.Product.Price:N2}\t{bestSell.Num}");

            //
            // 2) สินค้าที่ราคาสูงที่สุด และขายได้กี่ชิ้น
            //
            var highestPrice = allItems
                .GroupBy(i => i.Product.Id)
                .Select(g => new
                {
                    Product = g.First().Product,
                    Num = g.Sum(x => x.Qty)
                })
                .OrderByDescending(x => x.Product.Price)
                .First();

            Console.WriteLine("\n2. Expensive price product and number");
            Console.WriteLine("ID\tNAME\t\tCATE\tPrice\tNum");
            Console.WriteLine($"{highestPrice.Product.Id}\t{highestPrice.Product.Name}\t{highestPrice.Product.Category}\t{highestPrice.Product.Price:N2}\t{highestPrice.Num}");

            //
            // 3) ราคาขายเฉลี่ยของยอดสุทธิทั้งหมด
            //

            double sumNet = 0;

            foreach (var o in orders)
            {
                double sum = 0, discount = 0;

                // รวมราคาแบบง่าย
                foreach (var item in o.Items)
                    sum += item.Product.Price * item.Qty;

                // ส่วนลดแบบซื้อเป็นคู่ ๆ ต่อรหัสสินค้า
                var rows = o.Items.GroupBy(i => i.Product.Id)
                    .Select(g => new { Product = g.First().Product, Qty = g.Sum(x => x.Qty) });

                foreach (var row in rows)
                {
                    int pair = row.Qty / 2;
                    if (pair > 0)
                        discount += (row.Product.Price * 2 * pair) * 0.10;
                }

                double net = sum - discount;
                sumNet += net;
            }

            double avg = sumNet / orders.Count;

            Console.WriteLine("\n3. Average sales price of all orders");
            Console.WriteLine($"Sum = {sumNet:N2}, Count = {orders.Count}, Average = {avg:N2}");
        }



    }
}
