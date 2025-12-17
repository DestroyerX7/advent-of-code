using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2025.Day06;

[PuzzleName("Trash Compactor")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        string[][] array = new string[input.Length][];

        for (int i = 0; i < input.Length; i++)
        {
            array[i] = input[i].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        }

        long total = 0;

        for (int x = 0; x < array[0].Length; x++)
        {
            string operation = "";
            long num = -1;

            for (int y = input.Length - 1; y >= 0; y--)
            {
                if (y == input.Length - 1)
                {
                    operation = array[y][x];

                    num = operation == "*" ? 1 : 0;
                    continue;
                }

                if (operation == "*")
                {
                    num *= int.Parse(array[y][x]);
                }
                else
                {
                    num += int.Parse(array[y][x]);
                }
            }

            total += num;
        }

        return total;
    }

    public override object SolvePartTwo(string[] input)
    {
        long total = 0;

        List<string> numStrings = [];
        string operation = "";

        for (int x = 0; x < input[0].Length; x++)
        {
            numStrings.Add("");

            bool allSpaces = true;

            for (int y = 0; y < input.Length; y++)
            {
                if (input[y][x] == ' ')
                {
                    continue;
                }

                allSpaces = false;

                if (input[y][x] == '*')
                {
                    operation = "*";
                    continue;
                }
                else if (input[y][x] == '+')
                {
                    operation = "+";
                    continue;
                }

                numStrings[^1] += input[y][x].ToString();
            }

            if (allSpaces || x == input[0].Length - 1)
            {
                numStrings.Remove("");

                if (operation == "*")
                {
                    long result = 1;

                    foreach (string numString in numStrings)
                    {
                        result *= int.Parse(numString);
                    }

                    total += result;
                }
                else
                {
                    foreach (string numString in numStrings)
                    {
                        total += int.Parse(numString);
                    }
                }

                operation = "";
                numStrings.Clear();
            }
        }

        return total;
    }
}