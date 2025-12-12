using System;
using System.Collections.Generic;
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
        HashSet<Vector2> redTilePositions = [];

        foreach (string line in input)
        {
            int[] split = [.. line.Split(',').Select(int.Parse)];
            Vector2 redTilePos = new(split[0], split[1]);
            redTilePositions.Add(redTilePos);
        }

        return 0;

        // System.Console.WriteLine(redTilePositions.MaxBy(r => r.X));
        // System.Console.WriteLine(redTilePositions.MaxBy(r => r.Y));

        // for (long i = 0; i < 9000000000; i++)
        // {
        //     int hi = 8 + 1;
        //     hi *= 10;
        // }

        // return 0;
        // bool previous = false;
        // Vector2 previousRedTilePos = new();

        // for (int i = 0; i < input.Length; i++)
        // {
        //     int[] split = [.. input[i].Split(',').Select(int.Parse)];
        //     Vector2 redTilePos = new(split[0], split[1]);

        //     if (previous)
        //     {
        //         Wall wall = new(previousRedTilePos, redTilePos);
        //     }

        //     previousRedTilePos = redTilePos;
        //     previous = true;
        // }

        // return "Not Attempted Yet";
    }
}

public struct Wall(Vector2 redTileone, Vector2 redTileTwo)
{
    public Vector2 RedTileOne = redTileone;
    public Vector2 RedTileTwo = redTileTwo;
}