using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024.Day13;

[PuzzleName("Claw Contraption")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Regex regex = new(@"\d+");

        int aButtonCost = 3;
        int bButtonCost = 1;

        int fewestTokens = 0;

        for (int i = 0; i < input.Length; i += 4)
        {
            MatchCollection aButtonMatches = regex.Matches(input[i]);
            MatchCollection bButtonMatches = regex.Matches(input[i + 1]);
            MatchCollection prizePosMatches = regex.Matches(input[i + 2]);

            int[] aButton = aButtonMatches.Select(m => int.Parse(m.Value)).ToArray();
            int[] bButton = bButtonMatches.Select(m => int.Parse(m.Value)).ToArray();
            int[] prizePos = prizePosMatches.Select(m => int.Parse(m.Value)).ToArray();

            bool solutionExists = false;
            int cheapest = int.MaxValue;

            for (int j = 0; j <= 100; j++)
            {
                int[] remainingDistance = { prizePos[0] - j * aButton[0], prizePos[1] - j * aButton[1] };

                if (remainingDistance[0] % bButton[0] != 0 || remainingDistance[1] % bButton[1] != 0)
                {
                    continue;
                }

                int bPressesNeeded = remainingDistance[0] / bButton[0];

                if (bButton[1] * bPressesNeeded != remainingDistance[1])
                {
                    continue;
                }

                solutionExists = true;
                int cost = j * aButtonCost + bPressesNeeded * bButtonCost;

                if (cost < cheapest)
                {
                    cheapest = cost;
                }
            }

            if (solutionExists)
            {
                fewestTokens += cheapest;
            }
        }

        return fewestTokens;
    }

    public override object SolvePartTwo(string[] input)
    {
        Regex regex = new(@"\d+");

        int aButtonCost = 3;
        int bButtonCost = 1;

        long fewestTokens = 0;

        for (int i = 0; i < input.Length; i += 4)
        {
            MatchCollection aButtonMatches = regex.Matches(input[i]);
            MatchCollection bButtonMatches = regex.Matches(input[i + 1]);
            MatchCollection prizePosMatches = regex.Matches(input[i + 2]);

            int[] aButton = aButtonMatches.Select(m => int.Parse(m.Value)).ToArray();
            int[] bButton = bButtonMatches.Select(m => int.Parse(m.Value)).ToArray();
            long[] prizePos = prizePosMatches.Select(m => long.Parse(m.Value)).ToArray();
            prizePos[0] += 10000000000000;
            prizePos[1] += 10000000000000;

            double divided = (double)bButton[1] / bButton[0];
            double aPresses = (divided * prizePos[0] - prizePos[1]) / (divided * aButton[0] - aButton[1]);
            double bPresses = (prizePos[1] - aPresses * aButton[1]) / bButton[1];
            aPresses = Math.Round(aPresses, 3);
            bPresses = Math.Round(bPresses, 3);

            if (double.IsInteger(aPresses) && double.IsInteger(bPresses))
            {
                fewestTokens += (long)aPresses * aButtonCost + (long)bPresses * bButtonCost;
            }
        }

        return fewestTokens;
    }
}