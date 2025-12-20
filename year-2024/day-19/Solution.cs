using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day19;

[PuzzleName("Linen Layout")]
public class Solution : Solver
{
    private static readonly Dictionary<string, long> _patternToNumWays = new();

    public override object SolvePartOne(string[] input)
    {
        List<string> patterns = input[0].Split(", ").ToList();

        int numPossible = 0;

        for (int i = 2; i < input.Length; i++)
        {
            if (IsPossible(input[i], patterns))
            {
                numPossible++;
            }
        }

        return numPossible;
    }

    public override object SolvePartTwo(string[] input)
    {
        _patternToNumWays.Clear(); // Added in 2025
        List<string> patterns = input[0].Split(", ").ToList();

        long numWays = 0;

        for (int i = 2; i < input.Length; i++)
        {
            numWays += NumWays(input[i], patterns);
        }

        return numWays;
    }

    private static bool IsPossible(string design, List<string> patterns)
    {
        if (design.Length == 0)
        {
            return true;
        }

        foreach (string pattern in patterns)
        {
            if (design.IndexOf(pattern) == 0 && IsPossible(design[pattern.Length..], patterns))
            {
                return true;
            }
        }

        return false;
    }

    // Given a design and a list of patterns, returns the number of ways the design can be constructed using the patterns
    private static long NumWays(string design, List<string> patterns)
    {
        if (design.Length == 0)
        {
            return 1;
        }

        long totalNumWays = 0;

        foreach (string pattern in patterns)
        {
            if (design.IndexOf(pattern) == 0)
            {
                string remainingPattern = design[pattern.Length..];

                if (!_patternToNumWays.ContainsKey(remainingPattern))
                {
                    _patternToNumWays[remainingPattern] = NumWays(remainingPattern, patterns);
                }

                totalNumWays += _patternToNumWays[remainingPattern];
            }
        }

        return totalNumWays;
    }
}