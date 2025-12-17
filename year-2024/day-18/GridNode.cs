using System.Collections.Generic;

namespace AdventOfCode.Year2024.Day18;

public class GridNode
{
    public Vector2 Position { get; private set; }
    public bool IsCurrupted { get; private set; }
    public List<GridNode> Connections = new();

    public int GCost = int.MaxValue;
    public int HCost = int.MaxValue;
    public int FCost => GCost + HCost;
    public bool IsVisited;

    public GridNode(Vector2 pos, bool isCurrupted)
    {
        Position = pos;
        IsCurrupted = isCurrupted;
    }

    public void AddConnection(GridNode gridNode)
    {
        Connections.Add(gridNode);
    }

    public void Currupt()
    {
        IsCurrupted = true;

        foreach (GridNode connection in Connections)
        {
            connection.Connections.Remove(this);
        }
    }

    public void Reset()
    {
        GCost = int.MaxValue;
        HCost = int.MaxValue;
        IsVisited = false;
    }
}