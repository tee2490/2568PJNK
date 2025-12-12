using TheaterApp4.Models;

namespace TheaterApp4.Services
{
    public class TheatreService : ITheatreService
    {
        Random r;
        public List<List<Ticket>> Theatres;

        public List<ReportByQuarter> ReportByQuarters = new();

        public TheatreService()
        {
            r = new Random();
            Theatres = new();
            CreateTheatre();
            Report();
        }

        public void CreateTheatre() //หลายโรง
        {

            var number = r.Next(2, 6);

            for (int i = 1; i <= number; i++)
            {
                Theatres.Add(CreateTicket());
            }
        }

        private List<Ticket> CreateTicket() // 1 โรง
        {
            var number = r.Next(10, 21);
            var tempTickets = new List<Ticket>();

            for (int i = 1; i <= number; i++)
            {
                tempTickets.Add(new Ticket
                {
                    Id = i,
                    Age = r.Next(3, 101),
                    Gentle = (SD.Sex)r.Next(0, 2),
                    MemberType = (SD.TypeM)r.Next(0, 2),
                    Month = r.Next(1, 13)
                });
            }

            return tempTickets;
        }

        public (KeyValuePair<int, int> maxMonth, KeyValuePair<int, int> minMonth) MaxMinMonth(List<Ticket> tickets)
        {
            var maxMonth = tickets.CountBy(p => p.Month).MaxBy(p => p.Value);
            var minMonth = tickets.CountBy(p => p.Month).MinBy(p => p.Value);

            return (maxMonth, minMonth);
        }


        public void Report()
        {
            ReportByQuarters.Clear();

            var allTickets = Theatres.SelectMany(t => t);

            int quarter = 1;

            for (int start = 1; start <= 12; start += 3)
            {
                int stop = start + 2;

                var inQuarter = allTickets
                    .Where(t => t.Month >= start && t.Month <= stop)
                    .ToList();

                var report = new ReportByQuarter
                {
                    MonthRange = $"Month {start} - {stop} (Q{quarter})",
                    SumNet = inQuarter.Sum(t => t.Net),
                    CountMember = inQuarter.Count(t => t.MemberType == SD.TypeM.member),
                    CountGeneral = inQuarter.Count(t => t.MemberType == SD.TypeM.general),
                };

                ReportByQuarters.Add(report);
                quarter++;
            }
        }


        public void DisplayAll()
        {
            foreach (var item in Theatres)
            {
                Console.WriteLine($"\nTheater {Theatres.IndexOf(item) + 1}");
                DisplayTable(item);
                DisplaySumary(item);
            }

        }

        private void DisplaySumary(List<Ticket> tickets)
        {
            int maleCount = tickets.Count(t => t.Gentle.Equals(SD.Sex.M));
            int femaleCount = tickets.Count(t => t.Gentle.Equals(SD.Sex.F));

            int memberCount = tickets.Count(t => t.MemberType.Equals(SD.TypeM.member));
            int generalCount = tickets.Count(t => t.MemberType.Equals(SD.TypeM.general));

            var (maxMonth, minMonth) = MaxMinMonth(tickets);

            double avgAge = tickets.Average(t => t.Age);

            Console.WriteLine($"จำนวนเพศชาย (Count number of Male = {maleCount})");
            Console.WriteLine($"จำนวนเพศหญิง (Count number of Female = {femaleCount})");
            Console.WriteLine($"จำนวนบัตรสมาชิก (Count number of member = {memberCount})");
            Console.WriteLine($"จำนวนบัตรบุคคลทั่วไป (Count number of general = {generalCount})");

            Console.WriteLine(
                $"เดือนที่มีคนเข้าชมมากที่สุด (MAX ; Month = {maxMonth.Key} (จำนวน) number = {maxMonth.Value})"
            );

            Console.WriteLine(
                $"เดือนที่มีคนเข้าชมน้อยที่สุด (MIN ; Month = {minMonth.Key} (จำนวน) number = {minMonth.Value})"
            );

            Console.WriteLine($"ค่าเฉลี่ยอายุ (Average of Age = {avgAge:F2})");


            //เขียน แบบยาว
            // var monthGroups = theatres
            //.GroupBy(x => x.Month)
            //.Select(g => new { Month = g.Key, Count = g.Count() })
            //.ToList();

            // var maxMonth = monthGroups.OrderByDescending(x => x.Count).ThenBy(x => x.Month).FirstOrDefault();
            // var minMonth = monthGroups.OrderBy(x => x.Count).ThenBy(x => x.Month).FirstOrDefault();


        }


        private void DisplayTable(List<Ticket> item)
        {
            foreach (var t in item)
            {
                Console.WriteLine($"{t.Id,-5} {t.Gentle,-5} {t.Age,-5} {t.MemberType,-10}" +
                    $" {t.Month,-5} {t.Price,10} {t.Discount(),10:F2} {t.Net,10:F2}");
            }

        }

        public void DisplayByQuarter()
        {
            Console.WriteLine("\nรายงานสรุปรายไตรมาส (Report of each Quarter)");
            Console.WriteLine();

            Console.WriteLine(
            $"{"Month Range",-18}" +
            $"{"SUM NET",18}" +
            $"{"Count Member",24}" +
            $"{"Count General",26}" +
            $"{"Count All",24}"
        );
            Console.WriteLine(new string('-', 110));


            foreach (var r in ReportByQuarters)
            {
                Console.WriteLine(
                $"{r.MonthRange,-18}" +
                $"{r.SumNet,18:F2}" +
                $"{r.CountMember,24}" +
                $"{r.CountGeneral,26}" +
                $"{r.CountAll,24}"
            );
            }

        }

    }
}



//ถ้าต้องการดึงค่าทั้งหมดจาก Theatres ออกมาเป็น Ticket ทำอย่างไร
//ChatGPT said:

//Theatres เป็น List<List<Ticket>>
//ถ้าต้องการดึง Ticket ทั้งหมดออกมาเป็นลิสต์เดียว วิธีที่ถูกต้องและสั้นที่สุดคือใช้ SelectMany

//วิธีที่แนะนำ

//List<Ticket> allTickets = Theatres.SelectMany(t => t).ToList();


//ถ้าต้องการใช้งานทันทีโดยไม่สร้างตัวแปรใหม่

//var totalCount = Theatres.SelectMany(t => t).Count();


//หรือคำนวณยอดรวม Net

//double sumNet = Theatres.SelectMany(t => t).Sum(x => x.Net);

