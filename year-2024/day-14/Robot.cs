using System;

namespace AdventOfCode.Year2024.Day14;

public class Robot
{
    public Vector2 Position { get; private set; }
    private Vector2 _velocity;

    public Robot(Vector2 pos, Vector2 velocity)
    {
        Position = pos;
        _velocity = velocity;
    }

    public Vector2 MoveForSeconds(int seconds, int gridWidth, int gridHeight)
    {
        for (int i = 0; i < seconds; i++)
        {
            Position += _velocity;

            if (Position.X < 0)
            {
                Position = new(Position.X + gridWidth, Position.Y);
            }
            else if (Position.X >= gridWidth)
            {
                Position = new(Position.X % gridWidth, Position.Y);
            }

            if (Position.Y < 0)
            {
                Position = new(Position.X, Position.Y + gridHeight);
            }
            else if (Position.Y >= gridHeight)
            {
                Position = new(Position.X, Position.Y % gridHeight);
            }
        }

        return Position;
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
        return obj is Vector2 vector && vector.Equals(this);
    }

    // Added in 2025 to stop termianl warning
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}