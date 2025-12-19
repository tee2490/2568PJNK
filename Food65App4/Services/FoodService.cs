using Food65App4.Models;

namespace Food65App4.Services
{
    public class FoodService
    {
        public List<Food> Foods = new List<Food>();
        Random rand = new Random();

        public void GenerateData()
        {
            var count = rand.Next(20, 31);
            for (var i = 1; i <= count; i++)
            {
                var tempFood = new Food
                {
                    Id = i,
                    Name = $"Food_{i}",
                    Type = rand.Next(1, 6),
                    Cost = rand.Next(30,501) + rand.NextDouble(),
                    Cal = rand.Next(30, 201) + rand.NextDouble(),
                };
                tempFood.Cost = Math.Round(tempFood.Cost,2);
                tempFood.Cal = Math.Round(tempFood.Cal, 2);
                tempFood.CostRate = GetCostRate(tempFood.Cost);
                Foods.Add(tempFood);
            }
        }

        private string GetCostRate(double cost)
        {
            if (cost < 100) return "*";
            if (cost < 200) return "**";
            if (cost < 300) return "***";
            if (cost < 400) return "****";
            return "*****";
        }

        public void ShowData()
        {
            Foods.ForEach(p=> 
            {
                Console.WriteLine($"{p.Id,10} {p.Name,10} {p.Type,10} {p.Cost,10:F2} {p.Cal,10:F2} {p.CostRate,10}");
            });

            Console.WriteLine();
            Console.WriteLine($"{"Cost",20} {"Cal",10}");
            Console.WriteLine($"{"Max",10} {Foods.Max(p=>p.Cost),10:F2} {Foods.Max(p=>p.Cal),10:F2}");
            Console.WriteLine($"{"Min",10} {Foods.Min(p => p.Cost),10:F2} {Foods.Min(p => p.Cal),10:F2}");
            Console.WriteLine($"{"Average",10} {Foods.Average(p => p.Cost),10:F2} {Foods.Average(p => p.Cal),10:F2}");

        }

        public void ShowGroupByType()
        {
            var groups = Foods.GroupBy(a => a.Type).OrderBy(a => a.Key).ToList();

            foreach (var group in groups)
            {
                Console.WriteLine($"\nType {group.Key}:");
                foreach (var p in group)
                {
                    Console.WriteLine($"{p.Id,10} {p.Name,10} {p.Type,10} {p.Cost,10:F2} {p.Cal,10:F2} {p.CostRate,10}");
                }
                Console.WriteLine() ;
            }
        }

    }
}
