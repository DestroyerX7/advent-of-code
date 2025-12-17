using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day18;

[PuzzleName("RAM Run")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int width = 71;
        int height = 71;
        Grid grid = new(width, height);

        for (int i = 0; i < 1024; i++)
        {
            int commaIndex = input[i].IndexOf(',');
            int x = int.Parse(input[i][..commaIndex]);
            int y = int.Parse(input[i][(commaIndex + 1)..]);
            grid.SetAsCurrupted(x, y);
        }

        grid.UpdateConnections();

        return grid.GetLowestSteps(new(0, 0), new(width - 1, height - 1));
    }

    public override object SolvePartTwo(string[] input)
    {
        int width = 71;
        int height = 71;
        Grid grid = new(width, height);

        grid.UpdateConnections();

        for (int i = 0; i < input.Length; i++)
        {
            int commaIndex = input[i].IndexOf(',');
            int x = int.Parse(input[i][..commaIndex]);
            int y = int.Parse(input[i][(commaIndex + 1)..]);

            grid.SetAsCurrupted(x, y);

            int lowestSteps = grid.GetLowestSteps(new(0, 0), new(width - 1, height - 1));

            if (lowestSteps == -1)
            {
                return input[i];
            }
        }

        throw new System.Exception("Error");
    }
}