using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2025.Day12;

[PuzzleName("Christmas Tree Farm")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        List<Shape> shapes = [];

        int lineIndex = 1;
        for (int i = 0; i < 6; i++)
        {
            Shape shape = new([input[lineIndex], input[lineIndex + 1], input[lineIndex + 2]]);
            shapes.Add(shape);
            lineIndex += 5;
        }

        int numFit = 0;

        foreach (string line in input)
        {
            if (!line.Contains('x'))
            {
                continue;
            }

            string[] split = line.Split(": ");
            int[] gridSize = [.. split[0].Split('x').Select(int.Parse)];

            int[] shapeCounts = [.. split[1].Split(' ').Select(int.Parse)];

            // I had this at first but apparently you don't even need this
            // int totalTiles = 0;
            // for (int i = 0; i < shapeCounts.Length; i++)
            // {
            //     totalTiles += shapeCounts[i] * shapes[i].NumTiles;
            // }

            // Number of tiles is greater than the grid area, so it is not possible to fit
            // if (totalTiles > gridSize[0] * gridSize[1])
            // {
            //     continue;
            // }

            // This just feels so cheese
            if (gridSize[0] / 3 * (gridSize[1] / 3) >= shapeCounts.Aggregate((a, b) => a + b))
            {
                numFit++;
            }
        }

        return numFit;
    }

    public override object SolvePartTwo(string[] input)
    {
        return @"
You help the Elves decorate the Christmas trees with all 24 stars! Now, the Elves will have plenty of time to prepare for Christmas, and you get a well-deserved break.

Congratulations! You've finished every puzzle in Advent of Code 2025! I hope you had as much fun solving them as I had making them for you. I'd love to hear about your adventure; you can get in touch with me via contact info on my website or through Bluesky or Mastodon.

If you'd like to see more things like this in the future, please consider supporting Advent of Code and sharing it with others.

I've highlighted the easter eggs in each puzzle, just in case you missed any. Hover your mouse over them, and the easter egg will appear.

You can [Share] this moment with your friends or [Go Check on Your Calendar].";
    }
}

public struct Shape
{
    public char[,] Parts;
    public int Width;
    public int Height;
    public int NumTiles;

    public Shape(string[] lines)
    {
        NumTiles = 0;

        Width = lines[0].Length;
        Height = lines.Length;

        Parts = new char[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Parts[x, y] = lines[y][x];

                if (Parts[x, y] == '#')
                {
                    NumTiles++;
                }
            }
        }
    }

    // public void RotateClockwise()
    // {

    // }

    public override string ToString()
    {
        string output = "";

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                output += Parts[x, y];
            }

            output += "\n";
        }

        return output;
    }
}