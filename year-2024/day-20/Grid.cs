using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2024.Day20;

public class Grid
{
    private readonly GridNode[,] _gridNodes;
    private int Width => _gridNodes.GetLength(0);
    private int Height => _gridNodes.GetLength(1);

    public Grid(int width, int height)
    {
        _gridNodes = new GridNode[width, height];
    }

    public void SetGridPos(int x, int y, bool isWall)
    {
        Vector2 pos = new(x, y);
        _gridNodes[x, y] = new(pos, isWall);
    }

    public void UpdateIsWall(int x, int y, bool isWall)
    {
        _gridNodes[x, y].UpdateIsWall(isWall);

        if (!isWall)
        {
            Vector2 direction = Vector2.Right;
            for (int i = 0; i < 4; i++)
            {
                Vector2 connectionPos = new(x + direction.X, y + direction.Y);

                if (IsGridPos(connectionPos) && !_gridNodes[connectionPos.X, connectionPos.Y].IsWall)
                {
                    _gridNodes[x, y].AddConnection(_gridNodes[connectionPos.X, connectionPos.Y]);
                    _gridNodes[connectionPos.X, connectionPos.Y].AddConnection(_gridNodes[x, y]);
                }

                direction = direction.Rotate90DegreesClockwise();
            }
        }
    }

    public void UpdateConnections()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _gridNodes[x, y].Connections.Clear();

                if (_gridNodes[x, y].IsWall)
                {
                    continue;
                }

                Vector2 direction = Vector2.Right;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 connectionPos = new(x + direction.X, y + direction.Y);

                    if (IsGridPos(connectionPos) && !_gridNodes[connectionPos.X, connectionPos.Y].IsWall)
                    {
                        _gridNodes[x, y].AddConnection(_gridNodes[connectionPos.X, connectionPos.Y]);
                    }

                    direction = direction.Rotate90DegreesClockwise();
                }
            }
        }
    }

    public int GetFastestTime(Vector2 startPos, Vector2 endPos)
    {
        GridNode startNode = _gridNodes[startPos.X, startPos.Y];
        GridNode endNode = _gridNodes[endPos.X, endPos.Y];
        startNode.GCost = 0;
        startNode.HCost = (int)Math.Round(Vector2.Distance(startPos, endPos));

        PriorityQueue<GridNode, double> priorityQueue = new();
        priorityQueue.Enqueue(startNode, startNode.FCost);

        while (priorityQueue.Count > 0)
        {
            GridNode currentNode = priorityQueue.Dequeue();

            if (currentNode == endNode)
            {
                int gCost = currentNode.GCost;
                ResetNodes();
                return gCost;
            }

            if (currentNode.IsVisited)
            {
                continue;
            }

            currentNode.IsVisited = true;

            foreach (GridNode connection in currentNode.Connections)
            {
                if (!connection.IsVisited)
                {
                    int gCost = currentNode.GCost + 1;
                    connection.HCost = (int)Math.Round(Vector2.Distance(connection.Position, endPos));

                    if (gCost < connection.GCost)
                    {
                        connection.GCost = gCost;
                    }

                    priorityQueue.Enqueue(connection, connection.FCost);
                }
            }
        }

        ResetNodes();
        return -1;
    }

    private void ResetNodes()
    {
        foreach (GridNode gridNode in _gridNodes)
        {
            gridNode.Reset();
        }
    }

    public void Print()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (_gridNodes[x, y].IsWall)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }
    }

    private bool IsGridPos(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    private bool IsGridPos(Vector2 vector)
    {
        return IsGridPos(vector.X, vector.Y);
    }
}

public struct Vector2 : IEquatable<Vector2>
{
    public int X;
    public int Y;

    public static readonly Vector2 Right = new(1, 0);
    public static readonly Vector2 Left = new(-1, 0);
    public static readonly Vector2 Up = new(0, 1);
    public static readonly Vector2 Down = new(0, -1);
    public static readonly Vector2 Zero = new(0, 0);
    public static readonly Vector2 One = new(1, 1);

    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static Vector2 operator *(Vector2 vector, int num)
    {
        return new(vector.X * num, vector.Y * num);
    }

    public static Vector2 operator *(int num, Vector2 vector)
    {
        return vector * num;
    }

    public Vector2 Rotate90DegreesClockwise()
    {
        int oldX = X;
        return new(Y, -oldX);
    }

    public Vector2 Rotate90DegreesCounterclockwise()
    {
        int oldX = X;
        return new(-Y, oldX);
    }

    public static bool operator ==(Vector2 v1, Vector2 v2)
    {
        return v1.X == v2.X && v1.Y == v2.Y;
    }

    public static bool operator !=(Vector2 v1, Vector2 v2)
    {
        return v1.X != v2.X || v1.Y != v2.Y;
    }

    public static double Distance(Vector2 v1, Vector2 v2)
    {
        Vector2 delta = v2 - v1;
        return Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public bool Equals(Vector2 other)
    {
        return this == other;
    }

    // Added in 2025 to stop termianl warning
    public readonly override bool Equals(object? obj)
    {
        return obj is Vector2 vector && vector.Equals(this);
    }

    // Added in 2025 to stop termianl warning
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}