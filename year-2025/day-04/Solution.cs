using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2025.Day04;

[PuzzleName("Printing Department")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        char[][] grid = new char[input.Length][];

        for (int i = 0; i < input.Length; i++)
        {
            grid[i] = input[i].ToCharArray();
        }

        int width = input[0].Length;
        int height = input.Length;

        int numAccesable = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[y][x] != '@')
                {
                    continue;
                }

                int numAdjacent = 0;
                double[] direction = [1, 0];

                for (int i = 0; i < 8 && numAdjacent < 4; i++)
                {
                    Rotate45Degrees(direction);

                    int roundedX = (int)Math.Round(direction[0]);
                    int roundedY = (int)Math.Round(direction[1]);

                    if (x + roundedX < 0 || x + roundedX > width - 1 || y + roundedY < 0 || y + roundedY > height - 1)
                    {
                        continue;
                    }

                    if (grid[y + roundedY][x + roundedX] == '@')
                    {
                        numAdjacent++;
                    }
                }

                if (numAdjacent < 4)
                {
                    numAccesable++;
                }
            }
        }

        return numAccesable;
    }

    public override object SolvePartTwo(string[] input)
    {
        char[][] grid = new char[input.Length][];

        for (int i = 0; i < input.Length; i++)
        {
            grid[i] = input[i].ToCharArray();
        }

        int width = input[0].Length;
        int height = input.Length;

        int numRemoveable = 0;

        List<int[]> accessables = [];

        do
        {
            accessables.Clear();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[y][x] != '@')
                    {
                        continue;
                    }

                    int numAdjacent = 0;
                    double[] direction = [1, 0];

                    for (int i = 0; i < 8 && numAdjacent < 4; i++)
                    {
                        Rotate45Degrees(direction);

                        int roundedX = (int)Math.Round(direction[0]);
                        int roundedY = (int)Math.Round(direction[1]);

                        if (x + roundedX < 0 || x + roundedX > width - 1 || y + roundedY < 0 || y + roundedY > height - 1)
                        {
                            continue;
                        }

                        if (grid[y + roundedY][x + roundedX] == '@')
                        {
                            numAdjacent++;
                        }
                    }

                    if (numAdjacent < 4)
                    {
                        accessables.Add([x, y]);
                        numRemoveable++;
                    }
                }
            }

            foreach (int[] accessable in accessables)
            {
                grid[accessable[1]][accessable[0]] = '.';
            }
        }
        while (accessables.Count > 0);

        return numRemoveable;
    }

    private static void Rotate45Degrees(double[] direction)
    {
        //  0 1
        // -1 0

        // cos(45) sin(45)
        // -sin(45) cos(45)

        double x = direction[0];
        double y = direction[1];
        double rads = Math.Sqrt(2) / 2;
        direction[0] = x * rads + y * rads;
        direction[1] = x * -rads + y * rads;
    }
}