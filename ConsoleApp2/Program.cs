Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

// ------------------------------
// Part 1 : สุ่มสินค้า
// ------------------------------
Random rand = new Random();

// สุ่มจำนวนสินค้า 10–15
int N = rand.Next(10, 16);

// เตรียมค่าของ enum สำหรับสุ่มประเภทสินค้า
CategoryType[] categoryValues = (CategoryType[])Enum.GetValues(typeof(CategoryType));

List<Item> products = new();

for (int i = 1; i <= N; i++)
{
    double price = rand.Next(10,16)+ rand.NextDouble();

    products.Add(new Item(
        ID: i,
        Name: $"PRODUCT{i}",
        Category: categoryValues[rand.Next(categoryValues.Length)],
        Price: Math.Round(price, 2)   // ปัดทศนิยม 2 ตำแหน่ง
    ));
}

// แสดงสินค้า
Console.WriteLine("===== รายการสินค้า (N = " + N + ") =====\n");
PrintTable(products);

// ===================================================
// รายงาน 1 : สินค้าราคาสูงสุด
// ===================================================
Console.WriteLine("\n===== รายงาน 1 : สินค้าราคาสูงสุด =====\n");

double maxPrice = products.Max(p => p.Price);

var topItems = products.Where(p => p.Price == maxPrice)
                       .OrderBy(p => p.ID);

PrintTable(topItems);

// ===================================================
// รายงาน 2 : สินค้าราคาต่ำสุด
// ===================================================
Console.WriteLine("\n===== รายงาน 2 : สินค้าราคาต่ำสุด =====\n");

double minPrice = products.Min(p => p.Price);

var lowItems = products.Where(p => p.Price == minPrice)
                       .OrderBy(p => p.ID);

PrintTable(lowItems);

// ===================================================
// รายงาน 3 : รายงานสินค้าแยกตาม Category
// ===================================================
Console.WriteLine("\n===== รายงาน 3 : สินค้าแยกตามหมวดหมู่ =====\n");

var groups = products.GroupBy(p => p.Category)
                     .OrderBy(g => g.Key);

foreach (var g in groups)
{
    Console.WriteLine($"{g.Key} = {g.Count()}");
    PrintTable(g);
    Console.WriteLine();
}

// ===================================================
// Part 2 : สุ่มออเดอร์จากลูกค้า
// ===================================================
Console.WriteLine("\n===== Part 2 : สุ่มออเดอร์ของลูกค้า =====\n");

Console.Write("กรอกจำนวนลูกค้า (A) = ");
string? inputA = Console.ReadLine();
int A = int.TryParse(inputA, out int tmpA) && tmpA > 0 ? tmpA : 3;

Console.Write("กรอกจำนวนรายการสินค้าสูงสุดต่อคน (M) = ");
string? inputM = Console.ReadLine();
int M = int.TryParse(inputM, out int tmpM) && tmpM > 0 ? tmpM : 3;

Console.WriteLine();

// เก็บสินค้าทั้งหมดที่ถูกขาย (ทุกรายการ ทุกออเดอร์)
List<Item> allSoldItems = new();

for (int orderNo = 1; orderNo <= A; orderNo++)
{
    int itemCount = rand.Next(1, M + 1);

    List<Item> orderItems = new();

    for (int i = 0; i < itemCount; i++)
    {
        Item p = products[rand.Next(products.Count)];
        orderItems.Add(p);
    }

    allSoldItems.AddRange(orderItems);

    double sum = orderItems.Sum(p => p.Price);

    // คำนวณส่วนลดแบบเป็นคู่ (10% ต่อคู่)
    double discount = 0.0;

    var orderGroups = orderItems.GroupBy(p => p.ID);

    foreach (var g in orderGroups)
    {
        int qty = g.Count();
        int pairCount = qty / 2;

        if (pairCount > 0)
        {
            double unitPrice = g.First().Price;
            discount += pairCount * (2 * unitPrice * 0.10);
        }
    }

    double net = sum - discount;

    Console.WriteLine($"Order No.{orderNo}:");
    PrintOrderTable(orderItems);

    Console.WriteLine($"Sum      = {sum:F2}");
    Console.WriteLine($"Discount = {discount:F2}");
    Console.WriteLine($"Net      = {net:F2}");
    Console.WriteLine();
}

// ===================================================
// Part 3 : สรุปรายงานการขายทั้งหมด
// ===================================================
Console.WriteLine("\n===== Part 3 : สรุปรายงานการขายทั้งหมด =====\n");

if (allSoldItems.Count == 0)
{
    Console.WriteLine("ไม่มีรายการขาย ไม่สามารถสรุปรายงานได้");
}
else
{
    var soldSummary = allSoldItems
        .GroupBy(p => p.ID)
        .Select(g => new
        {
            Product = g.First(),
            Num = g.Count(),
            Total = g.Sum(x => x.Price)
        })
        .ToList();

    // 1. สินค้าที่ขายดีที่สุด (จำนวนชิ้นมากที่สุด)
    var mostSold = soldSummary
        .OrderByDescending(x => x.Num)
        .First();

    Console.WriteLine("1. สินค้าที่ขายดีที่สุด และจำนวนที่ขายได้\n");
    Console.WriteLine("ID\tNAME\t\tCATE\tPrice\tNum");
    Console.WriteLine(
        $"{mostSold.Product.ID}\t{mostSold.Product.Name}\t{mostSold.Product.Category}\t{mostSold.Product.Price:F2}\t{mostSold.Num}");
    Console.WriteLine();

    // 2. สินค้าที่ราคาต่อหน่วยแพงที่สุด และขายได้กี่ชิ้น
    var highestPriceProduct = soldSummary
        .OrderByDescending(x => x.Product.Price)
        .First();

    Console.WriteLine("2. สินค้าที่มีราคาต่อหน่วยแพงที่สุด และขายได้จำนวนเท่าไร\n");
    Console.WriteLine("ID\tNAME\t\tCATE\tPrice\tNum");
    Console.WriteLine(
        $"{highestPriceProduct.Product.ID}\t{highestPriceProduct.Product.Name}\t{highestPriceProduct.Product.Category}\t{highestPriceProduct.Product.Price:F2}\t{highestPriceProduct.Num}");
    Console.WriteLine();

    // 3. ราคาขายเฉลี่ยของรายการขายทั้งหมด (ยอดสุทธิรวม / จำนวนลูกค้า)
    double sumAll = allSoldItems.Sum(p=>p.Price);
    int countOrders = A;
    double avgAll = allSoldItems.Average(p=>p.Price);

    Console.WriteLine("3. ราคาขายเฉลี่ยของรายการขายทั้งหมด\n");
    Console.WriteLine($"Sum = {sumAll:F2}, Count = {countOrders}, Average = {avgAll:F2}");
}

// ------------------------------
// ฟังก์ชันแสดงตารางสินค้า
// ------------------------------
void PrintTable(IEnumerable<Item> items)
{
    Console.WriteLine("ID\tNAME\t\tCATE\tPrice");

    foreach (var p in items)
    {
        Console.WriteLine($"{p.ID}\t{p.Name}\t{p.Category}\t{p.Price:F2}");
    }
}

// ------------------------------
// ฟังก์ชันแสดงตาราง Order
// ------------------------------
void PrintOrderTable(IEnumerable<Item> items)
{
    Console.WriteLine("ID\tNAME\t\tCATE\tPrice");

    foreach (var p in items.OrderBy(x => x.ID))
    {
        Console.WriteLine($"{p.ID}\t{p.Name}\t{p.Category}\t{p.Price:F2}");
    }

    Console.WriteLine();
}

// ------------------------------
// ต้องวาง type ไว้ท้ายไฟล์เมื่อใช้ top-level statements
// ------------------------------
record Item(int ID, string Name, CategoryType Category, double Price);

enum CategoryType
{
    Drink,
    Food,
    General,
    Cloth
}
