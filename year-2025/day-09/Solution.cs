using System;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2025.Day09;

[PuzzleName("Movie Theater")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Vector2[] tilePositions = [.. input.Select(l =>
        {
            string[] split = l.Split(',');
            return new Vector2(int.Parse(split[0]), int.Parse(split[1]));
        })];

        long maxArea = 0;

        for (int i = 0; i < tilePositions.Length; i++)
        {
            for (int j = i + 1; j < tilePositions.Length; j++)
            {
                long width = Math.Abs(tilePositions[i].X - tilePositions[j].X) + 1;
                long height = Math.Abs(tilePositions[i].Y - tilePositions[j].Y) + 1;

                if (width * height > maxArea)
                {
                    maxArea = width * height;
                }
            }
        }

        return maxArea;
    }

    public override object SolvePartTwo(string[] input)
    {
        return "Not Attempted Yet";
    }
}