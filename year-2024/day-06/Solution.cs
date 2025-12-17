using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day06;

[PuzzleName("Guard Gallivant")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;

        Grid grid = new(width, height);
        int startX = 0;
        int startY = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid.SetGridNode(x, y, input[y][x]);

                if (input[y][x] == '^')
                {
                    startX = x;
                    startY = y;
                }
            }
        }

        return grid.GetNumPatrolPositions(startX, startY);
    }

    public override object SolvePartTwo(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;

        Grid grid = new(width, height);
        int startX = 0;
        int startY = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid.SetGridNode(x, y, input[y][x]);

                if (input[y][x] == '^')
                {
                    startX = x;
                    startY = y;
                }
            }
        }

        return grid.GetNumObstaclePlacementLocations(startX, startY);
    }
}