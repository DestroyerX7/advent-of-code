using System;

namespace AdventOfCode.Lib;

public struct Vector3(int x, int y, int z) : IEquatable<Vector3>
{
    public int X = x;
    public int Y = y;
    public int Z = z;

    public readonly double Magnitude => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));

    public static readonly Vector3 Up = new(0, 1, 0);
    public static readonly Vector3 Down = new(0, -1, 0);
    public static readonly Vector3 Left = new(-1, 0, 0);
    public static readonly Vector3 Right = new(1, 0, 0);
    public static readonly Vector3 Forward = new(0, 0, 1);
    public static readonly Vector3 Backward = new(0, 0, -1);
    public static readonly Vector3 One = new(1, 1, 1);
    public static readonly Vector3 Zero = new(0, 0, 0);

    public readonly bool Equals(Vector3 other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Vector3 vector && Equals(vector);
    }

    public static bool operator ==(Vector3 vectorOne, Vector3 vectorTwo)
    {
        return vectorOne.Equals(vectorTwo);
    }

    public static bool operator !=(Vector3 vectorOne, Vector3 vectorTwo)
    {
        return !vectorOne.Equals(vectorTwo);
    }

    public static Vector3 operator +(Vector3 vectorOne, Vector3 vectorTwo)
    {
        return new(vectorOne.X + vectorTwo.X, vectorOne.Y + vectorTwo.Y, vectorOne.Z + vectorTwo.Z);
    }

    public static Vector3 operator -(Vector3 vectorOne, Vector3 vectorTwo)
    {
        return new(vectorOne.X - vectorTwo.X, vectorOne.Y - vectorTwo.Y, vectorOne.Z - vectorTwo.Z);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public readonly override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}