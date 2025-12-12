using TheaterApp4.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var theaters = new TheatreService();

theaters.DisplayAll();
theaters.DisplayByQuarter();