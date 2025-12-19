using Score2563.Models;

namespace Score2563.Services
{
    public class StudentService
    {
        List<List<Student>> students = new List<List<Student>>();
        Random random = new Random();

        public void GenData(int room=2,int number=10)
        {
            for (int i = 1; i <= room; i++) 
            {
                var tempRoom = new List<Student>();
                for (int j = 1; j <= number; j++)
                { 
                    var student = new Student() 
                    {
                        Id = j,
                        Score = random.Next(10,21)
                    };
                    tempRoom.Add(student);
                }
                students.Add(tempRoom);

            }
        
        }


        public void Show() //แสดงผลแบบไม่ได้จัดรูปแบบ
        {
            foreach (var r in students)
            {
                var newR = r.GroupBy(p => p.Score).OrderByDescending(p=>p.Key).ToList();
                foreach (var item in newR)
                {
                    Console.Write(item.Key);
                    foreach (var item1 in item)
                    {
                        Console.Write($" {item1.Id} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

        }


        public void ShowFormat() //แสดงผลแบบจัดรูปแบบ
        {
            Console.WriteLine("Output");
            Console.WriteLine();

            for (int i = 0; i < students.Count; i++)
            {
                var room = students[i];

                Console.WriteLine($"Class {i + 1}");
                Console.WriteLine("Order\tScore\tID");

                int order = 1;

                var groups = room
                    .GroupBy(s => s.Score)
                    .OrderByDescending(g => g.Key);

                foreach (var g in groups)
                {
                    var ids = string.Join("\t", g.Select(x => x.Id).OrderBy(id => id));
                    Console.WriteLine($"{order}\t{g.Key}\t{ids}");
                    order++;
                }

                Console.WriteLine();
            }
        }

    }
}
