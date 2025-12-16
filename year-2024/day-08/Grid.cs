using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2024.Day08;

public class Grid
{
    private readonly GridNode[,] _grid;
    private int Width => _grid.GetLength(1);
    private int Height => _grid.GetLength(0);

    public Grid(int width, int height)
    {
        _grid = new GridNode[width, height];
    }

    public void SetGridNode(int x, int y, char character)
    {
        Vector2 pos = new(x, y);
        _grid[x, y] = new(pos, character);
    }

    public int GetNumAntinodeLocationsPartOne()
    {
        Dictionary<char, List<Vector2>> antennaLocations = new();

        foreach (GridNode gridNode in _grid)
        {
            if (!gridNode.IsAntenna)
            {
                continue;
            }

            if (antennaLocations.ContainsKey(gridNode.Character))
            {
                antennaLocations[gridNode.Character].Add(gridNode.Position);
            }
            else
            {
                antennaLocations[gridNode.Character] = new()
                {
                    gridNode.Position,
                };
            }
        }

        foreach (KeyValuePair<char, List<Vector2>> frequency in antennaLocations)
        {
            for (int i = 0; i < frequency.Value.Count - 1; i++)
            {
                for (int j = i + 1; j < frequency.Value.Count; j++)
                {
                    Vector2 distance = frequency.Value[j] - frequency.Value[i];

                    Vector2 pos1 = frequency.Value[j] + distance;
                    Vector2 pos2 = frequency.Value[i] - distance;

                    if (IsInGrid(pos1))
                    {
                        _grid[pos1.X, pos1.Y].SetIsAntinode(true);
                    }

                    if (IsInGrid(pos2))
                    {
                        _grid[pos2.X, pos2.Y].SetIsAntinode(true);
                    }
                }
            }
        }

        int num = 0;

        foreach (GridNode gridNode in _grid)
        {
            if (gridNode.IsAntinode)
            {
                num++;
            }
        }

        Reset();
        return num;
    }

    public int GetNumAntinodeLocationsPartTwo()
    {
        Dictionary<char, List<Vector2>> antennaLocations = new();

        foreach (GridNode gridNode in _grid)
        {
            if (!gridNode.IsAntenna)
            {
                continue;
            }

            if (antennaLocations.ContainsKey(gridNode.Character))
            {
                antennaLocations[gridNode.Character].Add(gridNode.Position);
            }
            else
            {
                antennaLocations[gridNode.Character] = new()
                {
                    gridNode.Position,
                };
            }
        }
        foreach (KeyValuePair<char, List<Vector2>> frequency in antennaLocations)
        {
            for (int i = 0; i < frequency.Value.Count - 1; i++)
            {
                for (int j = i + 1; j < frequency.Value.Count; j++)
                {
                    Vector2 distance = frequency.Value[j] - frequency.Value[i];

                    Vector2 pos1 = frequency.Value[j];
                    while (IsInGrid(pos1))
                    {
                        _grid[pos1.X, pos1.Y].SetIsAntinode(true);
                        pos1 += distance;
                    }

                    Vector2 pos2 = frequency.Value[i];
                    while (IsInGrid(pos2))
                    {
                        _grid[pos2.X, pos2.Y].SetIsAntinode(true);
                        pos2 -= distance;
                    }
                }
            }
        }

        int num = 0;

        foreach (GridNode gridNode in _grid)
        {
            if (gridNode.IsAntinode)
            {
                num++;
            }
        }

        Reset();
        return num;
    }

    private bool IsInGrid(Vector2 vector)
    {
        return vector.X >= 0 && vector.X < Width && vector.Y >= 0 && vector.Y < Height;
    }

    public void PrintGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (_grid[x, y].IsAntenna || !_grid[x, y].IsAntinode)
                {
                    Console.Write(_grid[x, y].Character);
                }
                else
                {
                    Console.Write("#");
                }
            }

            Console.WriteLine();
        }
    }

    private void Reset()
    {
        foreach (GridNode gridNode in _grid)
        {
            gridNode.SetIsAntinode(false);
        }
    }
}

public struct Vector2
{
    public int X;
    public int Y;

    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
    }
}