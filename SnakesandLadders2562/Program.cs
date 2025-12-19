Random rand = new Random();

// ตาราง Goto (ช่องพิเศษ)
int[] board = new int[26];   // index 1–25
board[1] = 8;
board[5] = 9;
board[8] = 20;
board[9] = 10;
board[11] = 17;
board[17] = 5;
board[20] = 4;
board[24] = 2;

int position = 0;

while (position < 25)
{
    int dice = rand.Next(1, 7);
    position += dice;

    Console.Write($"Dice={dice}, Position={position}");

    // ถ้ามี Goto ให้ย้ายต่อ
    while (position < 25 && board[position] != 0)
    {
        Console.Write($", Goto {board[position]}");
        position = board[position];
    }

    Console.WriteLine();
}

Console.WriteLine("Finish");