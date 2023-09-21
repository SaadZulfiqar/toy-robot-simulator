using System;
using System.IO;

namespace toy_robot_simulator
{
    public static class Direction
    {
        public const string NORTH = "NORTH";
        public const string EAST = "EAST";
        public const string WEST = "WEST";
        public const string SOUTH = "SOUTH";
    }
    public static class Command
    {
        public const string PLACE = "PLACE";
        public const string MOVE = "MOVE";
        public const string LEFT = "LEFT";
        public const string RIGHT = "RIGHT";
        public const string REPORT = "REPORT";
        public const string EXIT = "EXIT";
    }
    public class Program
    {
        // width of the square surface table
        static int table_Width = 5;
        // height of the square surface table
        static int table_Height = 5;
        // initialize robot's X position as -1 to represent that it's not placed on the table.
        static int position_X = -1;
        // initialize robot's Y position as -1 to represent that it's not placed on the table.
        static int position_Y = -1;
        // robot facing direction
        static string robotDirection = "";
        static string place_as_first_command = "";
        static void ExecuteCommand(string command)
        {
            string[] parts = command?.Trim().ToUpper().Split(' ');
            if (parts.Length == 0)
            {
                Console.WriteLine("Invalid command.");
                return;
            }

            if (string.IsNullOrEmpty(place_as_first_command))
            {
                if (parts[0] != Command.PLACE)
                {
                    Console.WriteLine("The first command has to be PLACE.");
                    return;
                }
                else
                {
                    place_as_first_command = Command.PLACE;
                }
            }

            switch (parts[0])
            {
                case Command.PLACE:
                    if (parts.Length == 2)
                    {
                        Place(parts[1]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid PLACE command.");
                    }
                    break;
                case Command.MOVE:
                    Move();
                    break;
                case Command.LEFT:
                    TurnLeft();
                    break;
                case Command.RIGHT:
                    TurnRight();
                    break;
                case Command.REPORT:
                    Report();
                    break;
                default:
                    {
                        Console.WriteLine("Invalid command.");
                    }
                    break;
            }
        }
        static void Place(string placement)
        {
            string[] coordinates = placement.Split(',');
            if (coordinates.Length != 3)
            {
                Console.WriteLine("Invalid placement.");
                return;
            }

            if (int.TryParse(coordinates[0], out int x) && int.TryParse(coordinates[1], out int y))
            {
                if (x >= 0 && x < table_Width && y >= 0 && y < table_Height)
                {
                    position_X = x;
                    position_Y = y;
                    robotDirection = coordinates[2];
                }
                else
                {
                    Console.WriteLine("Invalid placement, coordinates out of bounds.");
                }
            }
            else
            {
                Console.WriteLine("Invalid placement, coordinates are not integers.");
            }
        }
        static void Move()
        {
            if (position_X == -1 || position_Y == -1)
            {
                return;
            }

            int newX = position_X;
            int newY = position_Y;

            switch (robotDirection)
            {
                case Direction.NORTH:
                    newY++;
                    break;
                case Direction.SOUTH:
                    newY--;
                    break;
                case Direction.EAST:
                    newX++;
                    break;
                case Direction.WEST:
                    newX--;
                    break;
            }

            if (newX >= 0 && newX < table_Width && newY >= 0 && newY < table_Height)
            {
                position_X = newX;
                position_Y = newY;
            }
            else
            {
                Console.WriteLine("Invalid placement, coordinates out of bounds.");
            }
        }
        static void TurnLeft()
        {
            if (position_X == -1 || position_Y == -1)
            {
                return;
            }

            switch (robotDirection)
            {
                case Direction.NORTH:
                    robotDirection = Direction.WEST;
                    break;
                case Direction.WEST:
                    robotDirection = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    robotDirection = Direction.EAST;
                    break;
                case Direction.EAST:
                    robotDirection = Direction.NORTH;
                    break;

            }
        }
        static void TurnRight()
        {
            if (position_X == -1 || position_Y == -1)
                return;

            switch (robotDirection)
            {
                case Direction.NORTH:
                    robotDirection = Direction.EAST;
                    break;
                case Direction.EAST:
                    robotDirection = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    robotDirection = Direction.WEST;
                    break;
                case Direction.WEST:
                    robotDirection = Direction.NORTH;
                    break;
            }
        }
        static void Report()
        {
            if (position_X == -1 || position_Y == -1)
            {
                Console.WriteLine("Robot is not placed on the table.");
            }
            else
            {
                Console.WriteLine($"Robot is at ({position_X},{position_Y}) facing {robotDirection}");
            }
        }

        static void ReadFromFile(string inputFile)
        {
            if (File.Exists(inputFile))
            {
                string[] inputs = File.ReadAllLines(inputFile);
                foreach (string input in inputs)
                {
                    Console.WriteLine(input);
                    if (input?.ToUpper() == Command.EXIT)
                    {
                        break;
                    }
                    ExecuteCommand(input);
                }

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("No such file exists..");
            }
        }
        static void ReadFromConsole()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input?.ToUpper() == Command.EXIT)
                {
                    break;
                }

                ExecuteCommand(input);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("-------------- Toy Robot Simulator --------------");
            if (args.Length > 0)
            {
                // If command line arguments are provided, read the first argument as a file path (name);
                string filePath = args[0] ?? "commands.txt";
                ReadFromFile(filePath);
            }
            else
            {
                ReadFromConsole();
            }
        }
    }
}
