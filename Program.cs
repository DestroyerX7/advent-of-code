using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Lib;

namespace AdventOfCode;

public partial class Program
{
    private static readonly HttpClient _httpClient = new();
    private static readonly Regex _yearRegex = YearRegex();
    private static readonly Regex _dayRegex = DayRegex();

    private static async Task Main(string[] args)
    {
        await Run(args);
    }

    private static async Task Run(string[] args)
    {
        DateTime utcNow = DateTime.UtcNow.AddHours(-5);

        if (args.Length == 1 && args[0] == "today")
        {
            string year = utcNow.Year.ToString();
            string day = utcNow.Day.ToString("00");
            Solve(year, day);
        }
        else if (args.Length > 0 && args[0] == "create")
        {
            string year = utcNow.Year.ToString();
            string day = utcNow.Day.ToString("00");

            if (args.Length == 2 && _dayRegex.IsMatch(args[1]))
            {
                day = args[1].PadLeft(2, '0');
            }
            else if (args.Length == 3 && _yearRegex.IsMatch(args[1]) && _dayRegex.IsMatch(args[2]))
            {
                year = args[1];
                day = args[2].PadLeft(2, '0');
            }

            string folderPath = Path.Combine($"year-{year}", $"day-{day}");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);

                string solutionPath = Path.Combine(folderPath, "Solution.cs");
                string solutionTemplate = SolutionGenerator.GenerateSolutionTemplate(year, day);
                File.WriteAllText(solutionPath, solutionTemplate);

                string input = "";
                string inputUrl = $"https://adventofcode.com/{year}/day/{day.TrimStart('0')}/input";
                string session = Environment.GetEnvironmentVariable("ADVENT_OF_CODE_SESSION") ?? "";

                if (!string.IsNullOrEmpty(session))
                {
                    _httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session}");
                    HttpResponseMessage response = await _httpClient.GetAsync(inputUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        input = await response.Content.ReadAsStringAsync();
                        input = input.TrimEnd('\r', '\n');
                        Console.WriteLine($"✅ Loaded input.");
                    }
                    else
                    {
                        Console.WriteLine("❌ Failed to get input from AoC website. Make sure ADVENT_OF_CODE_SESSION is set correctly, and the year/day entered is correct.");
                    }
                }

                string inputPath = Path.Combine(folderPath, "input.in");
                File.WriteAllText(inputPath, input);
                Console.WriteLine($"✅ Successfully created day {day} solution template.");
            }
            else
            {
                Console.WriteLine("Folder path already exists : " + folderPath);
                Console.WriteLine("Nothing created.");
            }
        }
        else if (args.Length == 1 && args[0] == "all")
        {
            Solve();
        }
        else
        {
            string year = utcNow.Year.ToString();
            string day = utcNow.Day.ToString("00");

            if (args.Length == 1 && _dayRegex.IsMatch(args[0]))
            {
                day = args[0].PadLeft(2, '0');
            }
            else if (args.Length == 1 && _yearRegex.IsMatch(args[0]))
            {
                year = args[0];
                day = "All";
            }
            else if (args.Length == 2 && _yearRegex.IsMatch(args[0]) && _dayRegex.IsMatch(args[1]))
            {
                year = args[0];
                day = args[1].PadLeft(2, '0');
            }

            Solve(year, day);
        }
    }

    private static void Solve(string year = "All", string day = "All")
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly == null)
        {
            return;
        }

        Solver[] solvers = [.. entryAssembly.GetTypes().Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(Solver).IsAssignableFrom(t)).Select(t => (Solver)Activator.CreateInstance(t)!).Where(s => (s.GetYear() == year || year == "All") && (s.GetDay() == day || day == "All"))];

        if (solvers.Length == 0)
        {
            Console.WriteLine("Invalid year or day.");
            return;
        }

        foreach (Solver solver in solvers)
        {
            year = solver.GetYear();
            day = solver.GetDay();

            // Console.WriteLine($"Year {year}");
            // Console.WriteLine();

            string inputPath = Path.Combine($"year-{year}", $"day-{day}", "input.in");
            string[] input = File.ReadAllLines(inputPath);

            Stopwatch stopwatch = Stopwatch.StartNew();

            object partOneAnswer = solver.SolvePartOne(input);
            double partOneTime = stopwatch.ElapsedMilliseconds / 1000d;

            object partTwoAnswer = solver.SolvePartTwo(input);

            stopwatch.Stop();
            double partTwoTime = stopwatch.ElapsedMilliseconds / 1000d - partOneTime;

            WriteLine($"Day {solver.GetDay()} : {solver.GetName()}", ConsoleColor.Green);

            Write("Part One : ", ConsoleColor.Magenta);
            Console.WriteLine(partOneAnswer);

            Write("Part Two : ", ConsoleColor.Magenta);
            Console.WriteLine(partTwoAnswer);

            WriteLine("Solved part one in " + partOneTime + "s", ConsoleColor.Cyan);
            WriteLine("Solved part two in " + partTwoTime + "s", ConsoleColor.Cyan);
            WriteLine("Solved both parts in " + stopwatch.ElapsedMilliseconds / 1000d + "s", ConsoleColor.Cyan);
        }
    }

    public static void WriteLine(object? obj = null, ConsoleColor foregroundColor = ConsoleColor.White)
    {
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(obj);
        Console.ResetColor();
    }

    public static void Write(object? obj = null, ConsoleColor foregroundColor = ConsoleColor.White)
    {
        Console.ForegroundColor = foregroundColor;
        Console.Write(obj);
        Console.ResetColor();
    }

    [GeneratedRegex(@"^\d{4}$")]
    private static partial Regex YearRegex();

    [GeneratedRegex(@"^\d{1,2}$")]
    private static partial Regex DayRegex();
}
