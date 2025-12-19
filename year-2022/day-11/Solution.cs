using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day11;

[PuzzleName("Monkey in the Middle")]
public class Solution : Solver
{
    public static List<Monkey> Monkeys = new List<Monkey>();

    public override object SolvePartOne(string[] input)
    {
        foreach (string line in input)
        {
            if (line.Contains("Monkey"))
            {
                Monkeys.Add(new Monkey());
            }

            Monkey currentMonkey = Monkeys.Last();

            if (line.Contains("Starting items:"))
            {
                string items = line.Substring(18);
                currentMonkey.AddItems(items);
            }
            else if (line.Contains("Operation"))
            {
                currentMonkey.Operation = line;
            }
            else if (line.Contains("Test"))
            {
                currentMonkey.TestNum = long.Parse(line.Substring(line.LastIndexOf(" ") + 1));
            }
            else if (line.Contains("true"))
            {
                currentMonkey.TrueNum = long.Parse(line.Substring(line.LastIndexOf(" ") + 1));
            }
            else if (line.Contains("false"))
            {
                currentMonkey.FalseNum = long.Parse(line.Substring(line.LastIndexOf(" ") + 1));
            }
        }

        for (int i = 0; i < 20; i++)
        {
            foreach (Monkey monkey in Monkeys)
            {
                monkey.ChangeWorryLevels(null);
                monkey.ThrowItems();
            }
        }

        List<long> monkeyInspects = new List<long>();
        long[] topTwo = new long[2];

        foreach (var item in Monkeys)
        {
            monkeyInspects.Add(item.InspectNum);
        }

        topTwo[0] = monkeyInspects.Max();
        monkeyInspects.Remove(topTwo[0]);
        topTwo[1] = monkeyInspects.Max();

        return topTwo[0] * topTwo[1];
    }

    public override object SolvePartTwo(string[] input)
    {
        Monkeys.Clear(); // Added in 2025
        foreach (string line in input)
        {
            if (line.Contains("Monkey"))
            {
                Monkeys.Add(new Monkey());
            }

            Monkey currentMonkey = Monkeys.Last();

            if (line.Contains("Starting items:"))
            {
                string items = line.Substring(18);
                currentMonkey.AddItems(items);
            }
            else if (line.Contains("Operation"))
            {
                currentMonkey.Operation = line;
            }
            else if (line.Contains("Test"))
            {
                currentMonkey.TestNum = long.Parse(line.Substring(line.LastIndexOf(" ") + 1));
            }
            else if (line.Contains("true"))
            {
                currentMonkey.TrueNum = long.Parse(line.Substring(line.LastIndexOf(" ") + 1));
            }
            else if (line.Contains("false"))
            {
                currentMonkey.FalseNum = long.Parse(line.Substring(line.LastIndexOf(" ") + 1));
            }
        }

        long LCM = 1;

        foreach (var item in Monkeys)
        {
            LCM *= item.TestNum;
        }

        for (int i = 0; i < 10000; i++)
        {
            foreach (Monkey monkey in Monkeys)
            {
                monkey.ChangeWorryLevels(LCM);
                monkey.ThrowItems();
            }
        }

        List<long> monkeyInspects = new List<long>();
        long[] topTwo = new long[2];

        foreach (var item in Monkeys)
        {
            monkeyInspects.Add(item.InspectNum);
        }

        topTwo[0] = monkeyInspects.Max();
        monkeyInspects.Remove(topTwo[0]);
        topTwo[1] = monkeyInspects.Max();

        return topTwo[0] * topTwo[1];
    }
}