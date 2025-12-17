using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2023.Day02;

[PuzzleName("Cube Conundrum")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Dictionary<string, int> maxColorPairs = new()
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 },
        };

        int sumPossibleGames = 0;

        int currentGame = 1;

        foreach (string line in input)
        {
            string formatted = line[(line.IndexOf(':') + 1)..];
            string[] sets = formatted.Split(new char[] { ';', ',' });
            bool possibleGame = true;

            foreach (string set in sets)
            {
                string formattedNumColor = set.Trim();
                string numString = formattedNumColor.Substring(0, formattedNumColor.IndexOf(' '));
                string color = formattedNumColor[(formattedNumColor.IndexOf(' ') + 1)..];

                int num = int.Parse(numString);

                if (num > maxColorPairs[color])
                {
                    possibleGame = false;
                    break;
                }
            }

            if (possibleGame)
            {
                sumPossibleGames += currentGame;
            }

            currentGame++;
        }

        return sumPossibleGames;
    }

    public override object SolvePartTwo(string[] input)
    {
        int sum = 0;

        foreach (string line in input)
        {
            Dictionary<string, int> maxNumColors = new()
            {
                { "red", 0 },
                { "green", 0 },
                { "blue", 0 },
            };

            string formatted = line.Substring(line.IndexOf(':') + 1);
            string[] sets = formatted.Split(new char[] { ';', ',' });

            foreach (string set in sets)
            {
                string formattedNumColor = set.Trim();
                string numString = formattedNumColor[..formattedNumColor.IndexOf(' ')];
                string color = formattedNumColor[(formattedNumColor.IndexOf(' ') + 1)..];

                int num = int.Parse(numString);

                if (num > maxNumColors[color])
                {
                    maxNumColors[color] = num;
                }
            }

            int multiplied = maxNumColors["red"] * maxNumColors["green"] * maxNumColors["blue"];
            sum += multiplied;
        }

        return sum;
    }
}