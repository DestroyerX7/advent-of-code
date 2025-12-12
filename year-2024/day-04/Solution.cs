namespace AdventOfCode.Year2024.Day04;

[PuzzleName("Ceres Search")]
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
                grid.AddGridLetter(x, y, input[y][x]);
            }
        }

        return grid.GetNumAppearancesPartOne();
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
                grid.AddGridLetter(x, y, input[y][x]);
            }
        }

        return grid.GetNumAppearancesPartTwo();
    }
}