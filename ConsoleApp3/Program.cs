using ConsoleApp3.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var genData = new GenData();
int choice = 0;
do
{
    //Console.Clear();
    Console.WriteLine("===== เมนูหลัก =====");
    Console.WriteLine("1. สร้างข้อมูลจำลอง");
    Console.WriteLine("2. เพิ่มข้อมูล");
    Console.WriteLine("3. แก้ไขข้อมูล");
    Console.WriteLine("4. ลบข้อมูล");
    Console.WriteLine("5. เรียงสินค้าแบบต่างๆ");
    Console.WriteLine("9. ออกจากโปรแกรม");
    Console.Write("เลือกเมนู (1-9) = ");

    int.TryParse(Console.ReadLine(), out choice);
    Console.WriteLine();

    switch (choice)
    {
        case 1:
            genData.Create();
            genData.Show(GenData.products);
            break;
        case 2:
            genData.AddProduct();
            break;
        case 3:
            genData.EditProduct();
            break;
        case 4:
            genData.DeleteProduct();
            break;
        case 5:
            genData.SortProduct();
            break;
        case 9:
            Console.WriteLine("ออกจากโปรแกรม...");
            break;
        default:
            Console.WriteLine("เลือกเมนูไม่ถูกต้อง!");
            break;
    }

    if (choice != 9)
    {
        Console.WriteLine("\nกด Enter เพื่อกลับสู่เมนู...");
        Console.ReadLine();
    }

} while (choice != 9);