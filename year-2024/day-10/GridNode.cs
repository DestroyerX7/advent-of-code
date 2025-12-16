using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2024.Day10;

public class GridNode
{
    public Vector2 Position { get; private set; }
    public int Height { get; private set; }
    private readonly List<GridNode> _connections = new();

    public GridNode(Vector2 pos, int height)
    {
        Position = pos;
        Height = height;
    }

    public void AddConnection(GridNode gridNode)
    {
        _connections.Add(gridNode);
    }

    public int GetNumTrails(HashSet<GridNode> ninesReached)
    {
        if (Height == 9 && !ninesReached.Contains(this))
        {
            ninesReached.Add(this);
            return 1;
        }

        int numTrails = 0;

        foreach (GridNode gridNode in _connections)
        {
            numTrails += gridNode.GetNumTrails(ninesReached);
        }

        return numTrails;
    }

    public int GetNumDistinctTrails()
    {
        if (Height == 9)
        {
            return 1;
        }

        int numTrails = 0;

        foreach (GridNode gridNode in _connections)
        {
            numTrails += gridNode.GetNumDistinctTrails();
        }

        return numTrails;
    }

    public void PrintConnections()
    {
        Console.Write(Height + " : ");

        foreach (GridNode gridNode in _connections)
        {
            Console.Write(gridNode.Height + ", ");
        }

        Console.WriteLine();
    }
}