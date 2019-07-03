namespace Snakies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.IO;
    public class Snake
    {
        public static int x { get; set; }
        public static double xInv { get; set; }
        public static int y { get; set; }
        public static double yInv { get; set; }
        public static int length { get; set; }
        public static double moveX { get; set; }
        public static double moveY { get; set; }
        public static char head { get; set; }
        public static char tail { get; set; }
    }
    public class Treat
    {
        public static int x { get; set; }
        public static int y { get; set; }
        public static char matter { get; set; }
    }
    public class Game
    {
        public static int size { get;set; }
        public static int speed { get; set; }
        public static char sidesX { get; set; }
        public static char sidesY { get; set; }
        public static int highScore { get; set; }
    }
    public class Program
    {
        public static void Main()
        {
            var options = new Dictionary<int, string>();
            start:
            Console.Clear();
            using (var reader = new StreamReader("../../../../game.txt"))
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    Console.WriteLine("Welcome to Snake's menu, newcomer!");
                    goto skip;
                }
                else
                {
                    Console.WriteLine("Welcome to Snake's menu again!");
                    Console.WriteLine("Do you want your previous game options launched? Type: yes/no");
                    string answer = Console.ReadLine();
                    if (answer == "yes")
                    {
                        int index = 0;
                        while (line != null)
                        {
                            options[index] = line;
                            line = reader.ReadLine();
                            index++;
                        }
                        Game.size = int.Parse(options[0]);
                        Game.sidesX = char.Parse(options[1]);
                        Game.sidesY = char.Parse(options[2]);
                        Snake.head = char.Parse(options[3]);
                        Snake.tail = char.Parse(options[4]);
                        Treat.matter = char.Parse(options[5]);
                        Game.highScore = int.Parse(options[6]);
                        goto game;
                    }
                    else if (answer == "no")
                    {
                        goto skip;
                    }
                }
            }
            skip:
            Console.Write("1/6. Type a game size (min: 10/max: 20): ");
            options[0] = Console.ReadLine();
            Game.size = int.Parse(options[0]);

            Console.Write("2/6. Type the game's left & right side symbol: ");
            options[1] = Console.ReadLine();
            Game.sidesX = char.Parse(options[1]);

            Console.Write("3/6. Type the game's top & bottom side symbol: ");
            options[2] = Console.ReadLine();
            Game.sidesY = char.Parse(options[2]);

            Console.Write("4/6. Type a symbol for the Snake's head: ");
            options[3] = Console.ReadLine();
            Snake.head = char.Parse(options[3]);

            Console.Write("5/6. Type a symbol for the Snake's tail: ");
            options[4] = Console.ReadLine();
            Snake.tail = char.Parse(options[4]);

            Console.Write("6/6. Type a symbol for the food: ");
            options[5] = Console.ReadLine();
            Treat.matter = char.Parse(options[5]);


            game:
            int[,] map = new int[Game.size, Game.size];
            Random random = new Random();
            Game.speed = 4000;
            Snake.x = Game.size/2;
            Snake.y = Game.size/2;
            Snake.xInv = 0;
            Snake.yInv = 0;
            Snake.length = 4;
            Snake.moveX = 1d / Game.speed;
            Snake.moveY = 0;
            Treat.x = random.Next(3, Game.size - 3);
            Treat.y = random.Next(3, Game.size - 3);
            map[Snake.x, Snake.y] = Snake.length;
            map[Treat.x, Treat.y] = -1;
            Print(map);
            while (true)
            {
                Snake.xInv += Snake.moveX;
                Snake.yInv += Snake.moveY;
                if (Snake.xInv > 1 ||
                    Snake.xInv < -1 &&
                    Snake.moveY == 0)
                {
                    Snake.x += (int)Snake.xInv;
                    if (map[Snake.x, Snake.y] > 0)
                    {
                        map[Snake.x, Snake.y] = Snake.length;
                        Print(map);
                        Snake.xInv = 0;
                        Console.WriteLine("Ouch! You bit yourself!");
                        Death(options);
                        Console.WriteLine("Do you want to play again? Type: yes/no");
                        string input = Console.ReadLine();
                        if (input == "yes")
                        {
                            goto start;
                        }
                        else
                        {
                            break;
                        }
                    }
                    map[Snake.x, Snake.y] = Snake.length;
                    Print(map);
                    Snake.xInv = 0;
                }
                if (Snake.yInv > 1 ||
                    Snake.yInv < -1 &&
                    Snake.moveX == 0)
                {
                    Snake.y += (int)Snake.yInv;
                    if (map[Snake.x, Snake.y] > 0)
                    {
                        map[Snake.x, Snake.y] = Snake.length;
                        Print(map);
                        Snake.yInv = 0;
                        Console.WriteLine("Ouch! You bit yourself!");
                        Death(options);
                        Console.WriteLine("Do you want to play again? Type: yes/no");
                        string input = Console.ReadLine();
                        if (input == "yes")
                        {
                            goto start;
                        }
                        else
                        {
                            break;
                        }
                    }
                    map[Snake.x, Snake.y] = Snake.length;
                    Print(map);
                    Snake.yInv = 0;
                }
                if (Console.KeyAvailable &&
                    Console.ReadKey(true).Key == ConsoleKey.UpArrow &&
                    Snake.moveY == 0 &&
                    Snake.x != 0 &&
                    Snake.x != Game.size)
                {
                    Snake.moveY = -1d / Game.speed;
                    Snake.moveX = 0;
                    Snake.yInv = Snake.xInv < 0 ? Snake.xInv : Snake.xInv * (-1);
                    Snake.xInv = 0;
                }
                else if (Console.KeyAvailable &&
                    Console.ReadKey(true).Key == ConsoleKey.DownArrow &&
                    Snake.moveY == 0 &&
                    Snake.x != 0 &&
                    Snake.x != Game.size)
                {
                    Snake.moveY = 1d/Game.speed;
                    Snake.moveX = 0;
                    Snake.yInv = Snake.xInv < 0 ? Snake.xInv * (-1) : Snake.xInv;
                    Snake.xInv = 0;
                }
                else if (Console.KeyAvailable &&
                    Console.ReadKey(true).Key == ConsoleKey.LeftArrow &&
                    Snake.moveX == 0 &&
                    Snake.y != 0 &&
                    Snake.y != Game.size)
                {
                    Snake.moveY = 0;
                    Snake.moveX = -1d / Game.speed;
                    Snake.xInv = Snake.yInv < 0 ? Snake.yInv : Snake.yInv * (-1);
                    Snake.yInv = 0;
                }
                else if (Console.KeyAvailable &&
                    Console.ReadKey(true).Key == ConsoleKey.RightArrow &&
                    Snake.moveX == 0 &&
                    Snake.y != 0 &&
                    Snake.y != Game.size)
                {
                    Snake.moveY = 0;
                    Snake.moveX = 1d / Game.speed;
                    Snake.xInv = Snake.yInv < 0 ? Snake.yInv * (-1) : Snake.yInv;
                    Snake.yInv = 0;
                }

                if (Snake.x == Game.size - 2 &&
                    Snake.moveX > 0)
                {
                    Console.WriteLine("You bit a wall");
                    Death(options);
                    Console.WriteLine("Do you want to play again? Type: yes/no");
                    string input = Console.ReadLine();
                    if (input == "yes")
                    {
                        goto start;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (Snake.x == 1 &&
                    Snake.moveX < 0)
                {
                    Console.WriteLine("You bit a wall");
                    Death(options);
                    Console.WriteLine("Do you want to play again? Type: yes/no");
                    string input = Console.ReadLine();
                    if (input == "yes")
                    {
                        goto start;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (Snake.y == Game.size - 2 &&
                    Snake.moveY > 0)
                {
                    Console.WriteLine("You bit a wall");
                    Death(options);
                    Console.WriteLine("Do you want to play again? Type: yes/no");
                    string input = Console.ReadLine();
                    if (input == "yes")
                    {
                        goto start;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (Snake.y == 1 &&
                    Snake.moveY < 0)
                {
                    Console.WriteLine("You bit a wall!");
                    Death(options);
                    Console.WriteLine("Do you want to play again? Type: yes/no");
                    string input = Console.ReadLine();
                    if (input == "yes")
                    {
                        goto start;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (Snake.x == Treat.x &&
                    Snake.y == Treat.y)
                {
                    Snake.length++;
                    Treat.x = random.Next(3, Game.size - 3);
                    Treat.y = random.Next(3, Game.size - 3);
                    map[Treat.x, Treat.y] = -1;
                    Game.speed -= 100;
                    if (Game.highScore < Snake.length - 4)
                    {
                        Game.highScore = Snake.length - 4;
                    }
                    for (int y = 0; y < Game.size; y++)
                    {
                        for (int x = 0; x < Game.size; x++)
                        {
                            if (map[x,y] > 0)
                            {
                                map[x, y]++;
                            }
                        }
                    }
                }
            }
        }
        public static void Print(int[,] map)
        {
            Console.Clear();
            for (int y = 0; y < Game.size; y++)
            {
                for (int x = 0; x < Game.size; x++)
                {
                    if (map[x, y] > 0)
                    {
                        map[x, y]--;
                    }
                    if (y == 0 || y == Game.size - 1)
                    {
                        Console.Write(Game.sidesY);
                    }
                    else if (x == 0 || x == Game.size - 1)
                    {
                        Console.Write(Game.sidesX);
                    }
                    else
                    {
                        if (map[x, y] == 0)
                        {
                            Console.Write(" ");
                        }
                        else if (map[x, y] > 0 &&
                            map[x, y] < Snake.length - 1)
                        {
                            Console.Write(Snake.tail);
                        }
                        else if (map[x, y] == Snake.length - 1)
                        {
                            Console.Write(Snake.head);
                        }
                        else if (map[x,y] == -1)
                        {
                            Console.Write(Treat.matter);
                        }
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();   
            }
            Console.WriteLine($"High Score: {Game.highScore}");
            Console.WriteLine($"Score: {Snake.length-4}");
            Console.WriteLine($"Game Speed: {5d-Game.speed/1000d}");
        }
        public static void Death(Dictionary<int, string> options)
        {
            options[6] = Game.highScore.ToString();
            using (var writer = new StreamWriter("../../../../game.txt"))
            {
                for (int i = 0; i < options.Count; i++)
                {
                    writer.WriteLine(options[i]);
                }
            }
        }
    }
}
