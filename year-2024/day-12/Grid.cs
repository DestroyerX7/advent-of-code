using System;

namespace AdventOfCode.Year2024.Day12;

public class Grid
{
    private readonly GardenPlot[,] _gardenPlots;
    private int Width => _gardenPlots.GetLength(0);
    private int Height => _gardenPlots.GetLength(1);

    public Grid(int width, int height)
    {
        _gardenPlots = new GardenPlot[width, height];
    }

    public void SetGardenPlot(int x, int y, char plantType)
    {
        Vector2 pos = new(x, y);
        _gardenPlots[x, y] = new(pos, plantType);
    }

    public void UpdateConnections()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2 direction = Vector2.Right;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 connectionPos = new(x + direction.X, y + direction.Y);

                    if (IsGridPos(connectionPos) && _gardenPlots[connectionPos.X, connectionPos.Y].PlantType == _gardenPlots[x, y].PlantType)
                    {
                        _gardenPlots[x, y].AddConnection(_gardenPlots[connectionPos.X, connectionPos.Y]);
                    }

                    direction = direction.Rotate90DegreesClockwise();
                }
            }
        }
    }

    public int GetTotalPrice()
    {
        int totalPrice = 0;

        foreach (GardenPlot gardenPlot in _gardenPlots)
        {
            if (!gardenPlot.IsVisited)
            {
                RegionData regionData = gardenPlot.GetRegionData();
                totalPrice += regionData.Area * regionData.Perimeter;
            }
        }

        foreach (GardenPlot gardenPlot in _gardenPlots)
        {
            gardenPlot.Reset();
        }

        return totalPrice;
    }


    public int GetTotalBulkPrice()
    {
        int totalPrice = 0;

        foreach (GardenPlot gardenPlot in _gardenPlots)
        {
            if (!gardenPlot.IsVisited)
            {
                RegionData regionData = gardenPlot.GetRegionData();
                totalPrice += regionData.Area * regionData.SidesCount;
            }
        }

        foreach (GardenPlot gardenPlot in _gardenPlots)
        {
            gardenPlot.Reset();
        }

        return totalPrice;
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