using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day12;

[PuzzleName("Garden Groups")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        Grid grid = new(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid.SetGardenPlot(x, y, input[y][x]);
            }
        }

        grid.UpdateConnections();

        return grid.GetTotalPrice();
    }

    public override object SolvePartTwo(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        Grid grid = new(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid.SetGardenPlot(x, y, input[y][x]);
            }
        }

        grid.UpdateConnections();

        return grid.GetTotalBulkPrice();
    }
}