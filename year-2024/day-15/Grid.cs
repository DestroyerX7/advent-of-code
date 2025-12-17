using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2024.Day15;

public class Grid
{
    // Added ?s in 2025 to fix terminal warnings
    private readonly GridNode?[,] _gridNodes;
    private int Width => _gridNodes.GetLength(0);
    private int Height => _gridNodes.GetLength(1);

    public Grid(int width, int height)
    {
        _gridNodes = new GridNode[width, height];
    }

    public void SetGridPos(int x, int y, ItemType itemType)
    {
        _gridNodes[x, y] = new(itemType);
    }

    public bool TryMoveInDirectionPartOne(Vector2 pos, Vector2 direction)
    {
        if (_gridNodes[pos.X, pos.Y] == null)
        {
            return true;
        }

        if (_gridNodes[pos.X, pos.Y]?.ItemType == ItemType.Wall)
        {
            return false;
        }

        Vector2 other = pos + direction;

        bool canMove = TryMoveInDirectionPartOne(other, direction);

        if (canMove)
        {
            _gridNodes[other.X, other.Y] = _gridNodes[pos.X, pos.Y];
            _gridNodes[pos.X, pos.Y] = null;
        }

        return canMove;
    }

    public bool TryMoveInDirectionPartTwo(Vector2 pos, Vector2 direction)
    {
        if (direction == Vector2.Left || direction == Vector2.Right)
        {
            return TryMoveInDirectionPartOne(pos, direction);
        }

        HashSet<Vector2> visited = new();

        Queue<Vector2> queue = new();
        queue.Enqueue(pos + direction);

        while (queue.Count > 0)
        {
            int count = queue.Count;

            for (int i = 0; i < count; i++)
            {
                Vector2 currentPos = queue.Dequeue();
                visited.Add(currentPos);

                if (_gridNodes[currentPos.X, currentPos.Y] == null)
                {
                    continue;
                }
                else if (_gridNodes[currentPos.X, currentPos.Y]?.ItemType == ItemType.Wall)
                {
                    return false;
                }
                else if (_gridNodes[currentPos.X, currentPos.Y]?.ItemType == ItemType.LeftBoxPart)
                {
                    if (visited.All(v => v != currentPos + Vector2.Right))
                    {
                        queue.Enqueue(currentPos + Vector2.Right);
                    }

                    queue.Enqueue(currentPos + direction);
                }
                else if (_gridNodes[currentPos.X, currentPos.Y]?.ItemType == ItemType.RightBoxPart)
                {
                    if (visited.All(v => v != currentPos + Vector2.Left))
                    {
                        queue.Enqueue(currentPos + Vector2.Left);
                    }

                    queue.Enqueue(currentPos + direction);
                }
            }
        }

        MoveVertically(pos, direction);
        return true;
    }

    private void MoveVertically(Vector2 pos, Vector2 direction, bool moveOtherBoxPart = true)
    {
        if (_gridNodes[pos.X, pos.Y] == null)
        {
            return;
        }

        Vector2 other = pos + direction;

        MoveVertically(other, direction);

        if (_gridNodes[pos.X, pos.Y]?.ItemType == ItemType.RightBoxPart && moveOtherBoxPart)
        {
            MoveVertically(pos + Vector2.Left, direction, false);
        }
        else if (_gridNodes[pos.X, pos.Y]?.ItemType == ItemType.LeftBoxPart && moveOtherBoxPart)
        {
            MoveVertically(pos + Vector2.Right, direction, false);
        }

        _gridNodes[other.X, other.Y] = _gridNodes[pos.X, pos.Y];
        _gridNodes[pos.X, pos.Y] = null;
    }

    public int GetGPSCoordinateSum()
    {
        int sum = 0;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (_gridNodes[x, y] != null && (_gridNodes[x, y]?.ItemType == ItemType.Box || _gridNodes[x, y]?.ItemType == ItemType.LeftBoxPart))
                {
                    sum += 100 * y + x;
                }
            }
        }

        return sum;
    }

    private bool IsGridPos(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    private bool IsGridPos(Vector2 vector)
    {
        return IsGridPos(vector.X, vector.Y);
    }

    public void Print()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (_gridNodes[x, y] == null)
                {
                    Console.Write(".");
                }
                else if (_gridNodes[x, y]?.ItemType == ItemType.Box)
                {
                    Console.Write("O");
                }
                else if (_gridNodes[x, y]?.ItemType == ItemType.RightBoxPart)
                {
                    Console.Write("]");
                }
                else if (_gridNodes[x, y]?.ItemType == ItemType.LeftBoxPart)
                {
                    Console.Write("[");
                }
                else if (_gridNodes[x, y]?.ItemType == ItemType.Wall)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write("@");
                }
            }

            Console.WriteLine();
        }
    }
}

public struct Vector2
{
    public int X;
    public int Y;

    public static readonly Vector2 Right = new(1, 0);
    public static readonly Vector2 Left = new(-1, 0);
    public static readonly Vector2 Up = new(0, 1);
    public static readonly Vector2 Down = new(0, -1);

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

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    // Added in 2025 to stop termianl warning
    public readonly override bool Equals(object? obj)
    {
        return obj is Vector2 vector && vector.X == X && vector.Y == Y;
    }

    // Added in 2025 to stop termianl warning
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}