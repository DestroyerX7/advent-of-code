using System;

namespace AdventOfCode;

public struct Vector2(int x, int y) : IEquatable<Vector2>
{
    public int X = x;
    public int Y = y;

    public readonly double Magnitude => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

    public static readonly Vector2 Up = new(0, 1);
    public static readonly Vector2 Down = new(0, -1);
    public static readonly Vector2 Left = new(-1, 0);
    public static readonly Vector2 Right = new(1, 0);
    public static readonly Vector2 One = new(1, 1);
    public static readonly Vector2 Zero = new(0, 0);

    public readonly bool Equals(Vector2 other)
    {
        return X == other.X && Y == other.Y;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Vector2 vector && Equals(vector);
    }

    public static bool operator ==(Vector2 vectorOne, Vector2 vectorTwo)
    {
        return vectorOne.Equals(vectorTwo);
    }

    public static bool operator !=(Vector2 vectorOne, Vector2 vectorTwo)
    {
        return !vectorOne.Equals(vectorTwo);
    }

    public static Vector2 operator +(Vector2 vectorOne, Vector2 vectorTwo)
    {
        return new(vectorOne.X + vectorTwo.X, vectorOne.Y + vectorTwo.Y);
    }

    public static Vector2 operator -(Vector2 vectorOne, Vector2 vectorTwo)
    {
        return new(vectorOne.X - vectorTwo.X, vectorOne.Y - vectorTwo.Y);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public readonly override string ToString()
    {
        return $"({X}, {Y})";
    }
}