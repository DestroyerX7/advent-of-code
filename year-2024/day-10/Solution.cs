namespace AdventOfCode.Year2024.Day10;

[PuzzleName("Hoof It")]
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
                int num = int.Parse(input[y][x].ToString());
                grid.SetGridNode(x, y, num);
            }
        }

        grid.UpdateGridNodeConnections();

        return grid.GetSumTrailHeadScores();
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
                int num = int.Parse(input[y][x].ToString());
                grid.SetGridNode(x, y, num);
            }
        }

        grid.UpdateGridNodeConnections();

        return grid.GetSumTrailHeadRatings();
    }
}