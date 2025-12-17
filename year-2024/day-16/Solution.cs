using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day16;

[PuzzleName("Reindeer Maze")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        Grid grid = new(width, height);

        Vector2 startPos = new(-1, -1);
        Vector2 endPos = new(-1, -1);
        Vector2 startDirection = Vector2.Right;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pos = new(x, y);
                grid.SetGridNode(pos, input[y][x] == '#');

                switch (input[y][x])
                {
                    case 'S':
                        startPos = new(x, y);
                        break;
                    case 'E':
                        endPos = new(x, y);
                        break;
                }
            }
        }

        grid.UpdateConnections();

        return grid.GetLowestScore(startPos, endPos, startDirection);
    }

    public override object SolvePartTwo(string[] input)
    {
        return "Not Solved Yet";
    }
}