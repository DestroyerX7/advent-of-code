using System;

namespace AdventOfCode;

public struct Vector2(int x, int y) : IEquatable<Vector2>
{
    public int X = x;
    public int Y = y;

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