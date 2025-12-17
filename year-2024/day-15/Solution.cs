using System;
using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day15;

[PuzzleName("Warehouse Woes")]
public class Solution : Solver
{
    private static readonly Dictionary<char, Vector2> _directions = new()
    {
        { '>', Vector2.Right },
        { '<', Vector2.Left },
        { '^', Vector2.Down },
        { 'v', Vector2.Up },
    };

    public override object SolvePartOne(string[] input)
    {
        int width = input[0].Length;
        int height = Array.IndexOf(input, "");
        Grid grid = new(width, height);

        Vector2 robotPos = new(-1, -1);

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (y < height)
                {
                    switch (input[y][x])
                    {
                        case '@':
                            robotPos = new(x, y);
                            grid.SetGridPos(x, y, ItemType.Robot);
                            break;
                        case '#':
                            grid.SetGridPos(x, y, ItemType.Wall);
                            break;
                        case 'O':
                            grid.SetGridPos(x, y, ItemType.Box);
                            break;
                    }
                }
                else
                {
                    if (grid.TryMoveInDirectionPartOne(robotPos, _directions[input[y][x]]))
                    {
                        robotPos += _directions[input[y][x]];
                    }
                }
            }
        }

        return grid.GetGPSCoordinateSum();
    }

    public override object SolvePartTwo(string[] input)
    {
        int width = input[0].Length * 2;
        int height = Array.IndexOf(input, "");
        Grid grid = new(width, height);

        Vector2 robotPos = new(-1, -1);

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (y < height)
                {
                    switch (input[y][x])
                    {
                        case '@':
                            robotPos = new(x * 2, y);
                            grid.SetGridPos(x * 2, y, ItemType.Robot);
                            break;
                        case '#':
                            grid.SetGridPos(x * 2, y, ItemType.Wall);
                            grid.SetGridPos(x * 2 + 1, y, ItemType.Wall);
                            break;
                        case 'O':
                            grid.SetGridPos(x * 2, y, ItemType.LeftBoxPart);
                            grid.SetGridPos(x * 2 + 1, y, ItemType.RightBoxPart);
                            break;
                    }
                }
                else
                {
                    if (grid.TryMoveInDirectionPartTwo(robotPos, _directions[input[y][x]]))
                    {
                        robotPos += _directions[input[y][x]];
                    }
                }
            }
        }

        return grid.GetGPSCoordinateSum();
    }
}