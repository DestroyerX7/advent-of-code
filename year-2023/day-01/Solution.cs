using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2023.Day01;

[PuzzleName("Trebuchet?!")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int sum = 0;

        foreach (string line in input)
        {
            char first = line.First(char.IsDigit);
            char last = line.Last(char.IsDigit);

            string numAsString = string.Concat(new char[] { first, last });
            int finalNum = int.Parse(numAsString);
            sum += finalNum;
        }

        return sum;
    }

    public override object SolvePartTwo(string[] input)
    {
        Dictionary<string, int> digits = new()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };

        int sum = 0;

        foreach (string line in input)
        {
            int firstIndex = 1000;
            int lastIndex = -1;

            string first = "";
            string last = "";
            char[] split = line.ToCharArray();

            foreach (KeyValuePair<string, int> item in digits)
            {
                if (line.IndexOf(item.Key) != -1 && line.IndexOf(item.Key) < firstIndex)
                {
                    firstIndex = line.IndexOf(item.Key);
                    first = item.Value.ToString();
                }
            }

            for (int i = 0; i < split.Length; i++)
            {
                char letter = split[i];
                if (int.TryParse(letter.ToString(), out int num) && i < firstIndex)
                {
                    first = letter.ToString();
                    break;
                }
            }

            foreach (KeyValuePair<string, int> item in digits)
            {
                if (line.LastIndexOf(item.Key) != -1 && line.LastIndexOf(item.Key) > lastIndex)
                {
                    lastIndex = line.LastIndexOf(item.Key);
                    last = item.Value.ToString();
                }
            }

            for (int i = split.Length - 1; i >= 0; i--)
            {
                string letter = split[i].ToString();

                if (int.TryParse(letter, out int num) && i > lastIndex)
                {
                    last = letter;
                    break;
                }
            }

            string numAsString = first + last;
            int finalNum = int.Parse(numAsString);
            sum += finalNum;
        }

        return sum;
    }
}