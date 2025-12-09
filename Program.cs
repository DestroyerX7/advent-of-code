using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode;

public class Program
{
    private static void Main(string[] args)
    {
        DateTime utcNow = DateTime.UtcNow.AddHours(-5);

        if (args.Length > 0 && args[0] == "create")
        {
            string year = utcNow.Year.ToString();
            string day = utcNow.Day.ToString("00");

            if (args.Length > 1 && args[1].Length == 4)
            {
                year = args[1];
            }

            if (args.Length > 1 && args[1].Length == 2)
            {
                day = args[1];
            }

            if (args.Length > 2 && args[2].Length == 2)
            {
                day = args[2];
            }

            string folderPath = $"year-{year}/day-{day}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                File.Create(folderPath + "/input.in");

                string solutionTemplate = File.ReadAllText("SolutionTemplate.txt");

                File.WriteAllText(folderPath + "/Solution.cs", $"namespace AdventOfCode.Year{year}.Day{day};\n\n");
                File.AppendAllText(folderPath + "/Solution.cs", solutionTemplate);
            }
        }
        else
        {
            string year = utcNow.Year.ToString();
            string day = utcNow.Day.ToString("00");

            if (args.Length > 0 && args[0].Length == 4)
            {
                year = args[0];
            }

            if (args.Length > 0 && args[0].Length == 2)
            {
                day = args[0];
            }

            if (args.Length > 1 && args[1].Length == 2)
            {
                day = args[1];
            }

            Solve(year, day);
        }
    }

    private static void Solve(string year, string day)
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly == null)
        {
            return;
        }

        Solver? solver = entryAssembly.GetTypes().Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(Solver).IsAssignableFrom(t)).Select(t => (Solver)Activator.CreateInstance(t)!).FirstOrDefault(s => s.GetYear() == year && s.GetDay() == day);

        if (solver == null)
        {
            Console.WriteLine("Invalid year or day.");
            return;
        }

        string inputPath = Path.Combine($"year-{year}", $"day-{day}", "input.in");
        string[] input = File.ReadAllLines(inputPath);

        Stopwatch stopwatch = Stopwatch.StartNew();

        object partOneAnswer = solver.SolvePartOne(input);
        double partOneTime = stopwatch.ElapsedMilliseconds / 1000d;

        object partTwoAnswer = solver.SolvePartTwo(input);

        stopwatch.Stop();
        double partTwoTime = stopwatch.ElapsedMilliseconds / 1000d - partOneTime;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Day {solver.GetDay()} : {solver.GetName()}");

        Console.ForegroundColor = ConsoleColor.White;

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Part One : ");

        Console.ResetColor();
        Console.WriteLine(partOneAnswer);

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Part Two : ");

        Console.ResetColor();
        Console.WriteLine(partTwoAnswer);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Solved part one in " + partOneTime + "s");
        Console.WriteLine("Solved part two in " + partTwoTime + "s");
        Console.WriteLine("Solved both parts in " + stopwatch.ElapsedMilliseconds / 1000d + "s");

        Console.ResetColor();
        Console.Write("");
    }
}
