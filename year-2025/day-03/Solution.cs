using System;
using System.Linq;

namespace AdventOfCode.Year2025.Day03;

[ProblemName("")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int total = 0;

        foreach (string line in input)
        {
            int max = line.Select(c => int.Parse(c.ToString())).SkipLast(1).Max();
            int maxIndex = line.IndexOf(max.ToString());

            int next = line.Skip(maxIndex + 1).Select(c => int.Parse(c.ToString())).Max();

            int num = max * 10 + next;
            total += num;
        }

        return total;
    }

    public override object SolvePartTwo(string[] input)
    {
        long total = 0;

        foreach (string line in input)
        {
            string numString = "";
            int[] digits = [.. line.Select(c => int.Parse(c.ToString()))];

            int skipAmount = 0;

            for (int i = 0; i < 12; i++)
            {
                int max = digits[skipAmount..(line.Length - 12 + i + 1)].Max();
                int maxIndex = Array.IndexOf(digits[skipAmount..(line.Length - 12 + i + 1)], max);

                numString += max;
                skipAmount += maxIndex + 1;
            }

            total += long.Parse(numString);
        }

        return total;
    }
}