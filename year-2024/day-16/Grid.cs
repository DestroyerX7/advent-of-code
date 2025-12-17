using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2024.Day16;

public class Grid
{
    private readonly GridNode[,] _gridNodes;
    private int Width => _gridNodes.GetLength(0);
    private int Height => _gridNodes.GetLength(1);

    public Grid(int width, int height)
    {
        _gridNodes = new GridNode[width, height];
    }

    public void SetGridNode(Vector2 pos, bool isWall)
    {
        _gridNodes[pos.X, pos.Y] = new(pos, isWall);
    }

    public void UpdateConnections()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
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

    public int GetLowestScore(Vector2 startPos, Vector2 endPos, Vector2 startDirection)
    {
        GridNode startNode = _gridNodes[startPos.X, startPos.Y];
        GridNode endNode = _gridNodes[endPos.X, endPos.Y];

        AStarData aStarData = new(startDirection, 0, Vector2.Distance(startPos, endPos));
        startNode.DataFromDirection[startDirection] = aStarData;

        PriorityQueue<GridNode, double> priorityQueue = new();
        priorityQueue.Enqueue(startNode, aStarData.FCost);

        while (priorityQueue.Count > 0)
        {
            GridNode currentNode = priorityQueue.Dequeue();

            if (currentNode == endNode)
            {
                return currentNode.DataFromDirection.Min(d => d.Value.GCost);
            }

            foreach (GridNode connection in currentNode.Connections)
            {
                int gCost = currentNode.DataFromDirection.Min(d => d.Value.GCost) + 1;
            }
        }

        return int.MaxValue;
    }

    // public void Print()
    // {
    //     for (int y = 0; y < Height; y++)
    //     {
    //         for (int x = 0; x < Width; x++)
    //         {
    //             if (_gridNodes[x, y].IsPath && _gridNodes[x, y].Direction == Vector2.Right)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Green;
    //                 Console.Write(">");
    //             }
    //             else if (_gridNodes[x, y].IsPath && _gridNodes[x, y].Direction == Vector2.Left)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Green;
    //                 Console.Write("<");
    //             }
    //             else if (_gridNodes[x, y].IsPath && _gridNodes[x, y].Direction == Vector2.Up)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Green;
    //                 Console.Write("v");
    //             }
    //             else if (_gridNodes[x, y].IsPath && _gridNodes[x, y].Direction == Vector2.Down)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Green;
    //                 Console.Write("^");
    //             }
    //             else if (_gridNodes[x, y].IsWall)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.White;
    //                 Console.Write("#");
    //             }
    //             else
    //             {
    //                 Console.ForegroundColor = ConsoleColor.White;
    //                 Console.Write(".");
    //             }
    //         }

    //         Console.WriteLine();
    //     }
    // }

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