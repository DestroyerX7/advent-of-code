using System.Collections.Generic;

namespace AdventOfCode.Year2024.Day20;

public class GridNode
{
    public Vector2 Position { get; private set; }
    public bool IsWall { get; private set; }
    public List<GridNode> Connections = new();

    public int GCost = int.MaxValue;
    public int HCost = int.MaxValue;
    public int FCost => GCost + HCost;
    public bool IsVisited;

    public GridNode(Vector2 pos, bool isWall)
    {
        Position = pos;
        IsWall = isWall;
    }

    public void AddConnection(GridNode gridNode)
    {
        Connections.Add(gridNode);
    }

    public void UpdateIsWall(bool isWall)
    {
        IsWall = isWall;

        if (isWall)
        {
            foreach (GridNode connection in Connections)
            {
                connection.Connections.Remove(this);
            }

            Connections.Clear();
        }
    }

    public void Reset()
    {
        GCost = int.MaxValue;
        HCost = int.MaxValue;
        IsVisited = false;
    }
}