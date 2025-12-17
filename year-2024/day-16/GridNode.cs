using System.Collections.Generic;

namespace AdventOfCode.Year2024.Day16;

public class GridNode
{
    // Added = [] in 2025 to fix terminal warning
    public Dictionary<Vector2, AStarData> DataFromDirection = [];
    // public int GCost = int.MaxValue;
    // public double HCost = double.MaxValue;
    // public double FCost => GCost + HCost;
    public bool IsVisited;
    public Vector2 Direction;
    // public GridNode CameFrom;
    // public bool IsPath;

    public Vector2 Position;
    public bool IsWall { get; private set; }
    public List<GridNode> Connections = new();

    public GridNode(Vector2 pos, bool isWall)
    {
        Position = pos;
        IsWall = isWall;
    }

    public void AddConnection(GridNode gridNode)
    {
        Connections.Add(gridNode);
    }
}

public struct AStarData
{
    public Vector2 Direction;
    public int GCost = int.MaxValue;
    public double HCost = double.MaxValue;
    public double FCost => GCost + HCost;

    public AStarData(Vector2 direction, int gCost, double hCost)
    {
        Direction = direction;
        GCost = gCost;
        HCost = hCost;
    }
}