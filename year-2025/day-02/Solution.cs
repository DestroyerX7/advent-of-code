using System.Text.RegularExpressions;

namespace AdventOfCode.Year2025.Day02;

[PuzzleName("Gift Shop")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        string[] ranges = input[0].Split(",");
        long sum = 0;

        foreach (string range in ranges)
        {
            string[] split = range.Split("-");
            long startIndex = long.Parse(split[0]);
            long endIndex = long.Parse(split[1]);

            for (long i = startIndex; i <= endIndex; i++)
            {
                string currentIndex = i.ToString();
                if (currentIndex[..(currentIndex.Length / 2)] == currentIndex[(currentIndex.Length / 2)..])
                {
                    sum += i;
                }
            }
        }

        return sum;
    }

    public override object SolvePartTwo(string[] input)
    {
        string[] ranges = input[0].Split(",");
        long sum = 0;

        foreach (string range in ranges)
        {
            string[] split = range.Split("-");
            long startIndex = long.Parse(split[0]);
            long endIndex = long.Parse(split[1]);

            // I will try to fix the brute force and use math
            for (long i = startIndex; i <= endIndex; i++)
            {
                string currentIndex = i.ToString();

                for (int j = 1; j < currentIndex.Length / 2 + 1; j++)
                {
                    string pattern = currentIndex[..j];

                    MatchCollection matches = Regex.Matches(currentIndex, pattern);

                    if (matches.Count * j == currentIndex.Length)
                    {
                        sum += i;
                        break;
                    }
                }
            }
        }

        return sum;
    }
}