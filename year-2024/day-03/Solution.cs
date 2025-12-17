using System.Text.RegularExpressions;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day03;

[PuzzleName("Mull It Over")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        string pattern = @"mul\((\d+),(\d+)\)";
        Regex regex = new(pattern);

        int sum = 0;

        foreach (string line in input)
        {
            MatchCollection matches = regex.Matches(line);

            foreach (Match match in matches)
            {
                int numOne = int.Parse(match.Groups[1].Value);
                int numTwo = int.Parse(match.Groups[2].Value);
                sum += numOne * numTwo;
            }
        }

        return sum;
    }

    public override object SolvePartTwo(string[] input)
    {
        string pattern = @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)";
        Regex regex = new(pattern);

        int sum = 0;
        bool enabled = true;

        foreach (string line in input)
        {
            MatchCollection matches = regex.Matches(line);

            foreach (Match match in matches)
            {
                if (match.Value == "do()")
                {
                    enabled = true;
                }
                else if (match.Value == "don't()")
                {
                    enabled = false;
                }
                else if (enabled)
                {
                    int numOne = int.Parse(match.Groups[1].Value);
                    int numTwo = int.Parse(match.Groups[2].Value);
                    sum += numOne * numTwo;
                }
            }
        }

        return sum;
    }

    public object SolvePartOneOld(string[] input)
    {
        int sum = 0;

        foreach (string line in input)
        {
            string currentLine = line;
            currentLine = currentLine[currentLine.IndexOf("mul(")..];

            while (currentLine.Length >= 8 && currentLine.Contains("mul(") && currentLine.Contains(',') && currentLine.Contains(')'))
            {
                currentLine = currentLine[currentLine.IndexOf("mul(")..];

                int commaIndex = currentLine.IndexOf(',');
                int endIndex = currentLine.IndexOf(')');

                if (!int.TryParse(currentLine[4..commaIndex], out int numOne))
                {
                    currentLine = currentLine[4..];
                    continue;
                }

                if (!int.TryParse(currentLine[(commaIndex + 1)..endIndex], out int numTwo))
                {
                    currentLine = currentLine[4..];
                    continue;
                }

                sum += numOne * numTwo;
                currentLine = currentLine[4..];
            }
        }

        return sum;
    }

    public object SolvePartTwoOld(string[] input)
    {
        int sum = 0;
        bool enabled = true;

        foreach (string line in input)
        {
            // Holy sh*t, I spent like ~1.5 hrs trying to firgure out why my code didn't work.
            // I first tried the region using the while loop, but it wasn't giving me the right answer and I couldn't figure out what was wrong.
            // I then decided to try it a different way using the for loop, but it gave me the same answer.
            // I then looked on the AoC reddit to see if I was misreading the problem.
            // That is where I found out it only starts as enabled for the first line.
            // I was staring with it enabled on every new line.
            // I fixed this and both my solutions worked.
            // So dumb.

            #region Using for loop
            for (int i = 0; i < line.Length; i++)
            {
                string current = line[i..];

                if (!current.Contains("mul("))
                {
                    break;
                }

                if (current.IndexOf("do()") == 0)
                {
                    enabled = true;
                    i += 3;
                    continue;
                }
                else if (current.IndexOf("don't()") == 0)
                {
                    enabled = false;
                    i += 6;
                    continue;
                }
                else if (enabled && current.IndexOf("mul(") == 0 && current.Contains(',') && current.Contains(')'))
                {
                    int commaIndex = current.IndexOf(',');
                    int endIndex = current.IndexOf(')');

                    if (!int.TryParse(current[4..commaIndex], out int numOne))
                    {
                        i += 3;
                        continue;
                    }

                    if (!int.TryParse(current[(commaIndex + 1)..endIndex], out int numTwo))
                    {
                        i += 3;
                        continue;
                    }

                    sum += numOne * numTwo;
                    i += 3;
                }
            }
            #endregion

            #region Using while loop
            // string currentLine = line;

            // while (currentLine.Length >= 8 && currentLine.Contains("mul(") && currentLine.Contains(',') && currentLine.Contains(')'))
            // {
            //     int firstInstructionIndex = currentLine.IndexOf("mul(");

            //     if (currentLine.Contains("do()") && !currentLine.Contains("don't()"))
            //     {
            //         firstInstructionIndex = Math.Min(firstInstructionIndex, currentLine.IndexOf("do()"));
            //     }
            //     else if (!currentLine.Contains("do()") && currentLine.Contains("don't()"))
            //     {
            //         firstInstructionIndex = Math.Min(firstInstructionIndex, currentLine.IndexOf("don't()"));
            //     }
            //     else if (currentLine.Contains("do()") && currentLine.Contains("don't()"))
            //     {
            //         firstInstructionIndex = Math.Min(firstInstructionIndex, currentLine.IndexOf("do()"));
            //         firstInstructionIndex = Math.Min(firstInstructionIndex, currentLine.IndexOf("don't()"));
            //     }

            //     currentLine = currentLine[firstInstructionIndex..];

            //     if (currentLine.IndexOf("do()") == 0)
            //     {
            //         enabled = true;
            //         currentLine = currentLine[4..];
            //         continue;
            //     }
            //     else if (currentLine.IndexOf("don't()") == 0)
            //     {
            //         enabled = false;
            //         currentLine = currentLine[7..];
            //         continue;
            //     }

            //     if (!enabled)
            //     {
            //         currentLine = currentLine[4..];
            //         continue;
            //     }

            //     int commaIndex = currentLine.IndexOf(',');
            //     int endIndex = currentLine.IndexOf(')');

            //     if (!int.TryParse(currentLine[4..commaIndex], out int numOne))
            //     {
            //         currentLine = currentLine[4..];
            //         continue;
            //     }

            //     if (!int.TryParse(currentLine[(commaIndex + 1)..endIndex], out int numTwo))
            //     {
            //         currentLine = currentLine[4..];
            //         continue;
            //     }

            //     sum += numOne * numTwo;
            //     currentLine = currentLine[4..];
            // }
            #endregion
        }

        return sum;
    }
}