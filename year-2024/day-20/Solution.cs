using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day20;

[PuzzleName("Race Condition")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        Grid grid = new(width, height);

        Vector2 startPos = new(-1, -1);
        Vector2 endPos = new(-1, -1);

        List<Vector2> wallPositions = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid.SetGridPos(x, y, input[y][x] == '#');

                if (input[y][x] == 'S')
                {
                    startPos = new(x, y);
                }
                else if (input[y][x] == 'E')
                {
                    endPos = new(x, y);
                }
                else if (input[y][x] == '#')
                {
                    wallPositions.Add(new Vector2(x, y));
                }
            }
        }

        grid.UpdateConnections();

        int fastestTimeWithoutCheating = grid.GetFastestTime(startPos, endPos);
        int num = 0;

        foreach (Vector2 wallPos in wallPositions)
        {
            grid.UpdateIsWall(wallPos.X, wallPos.Y, false);

            int time = grid.GetFastestTime(startPos, endPos);

            if (fastestTimeWithoutCheating - time >= 100)
            {
                num++;
            }

            grid.UpdateIsWall(wallPos.X, wallPos.Y, true);
        }

        return num;
    }

    public override object SolvePartTwo(string[] input)
    {
        return "Not Solved Yet";
    }
}