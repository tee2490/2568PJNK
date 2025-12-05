using ConsoleApp1.Services;

var ps = new GenData();

ps.Show();
Console.WriteLine(new string('*', 50));

ps.ShowMaxPrice();
Console.WriteLine(new string('*', 50));

ps.ShowMinPrice();
Console.WriteLine(new string('*', 50));

ps.ShowByCategory();
Console.WriteLine(new string('*', 50));

ps.CreateOrders();
Console.WriteLine(new string('*', 50));
ps.ShowPart3();