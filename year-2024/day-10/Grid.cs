namespace AdventOfCode.Year2024.Day10;

public class Grid
{
    private readonly GridNode[,] _gridNodes;
    private int Width => _gridNodes.GetLength(1);
    private int Height => _gridNodes.GetLength(0);

    public Grid(int width, int height)
    {
        _gridNodes = new GridNode[width, height];
    }

    public void SetGridNode(int x, int y, int height)
    {
        Vector2 pos = new(x, y);
        _gridNodes[x, y] = new(pos, height);
    }

    public void UpdateGridNodeConnections()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2 direction = Vector2.Right;

                for (int i = 0; i < 4; i++)
                {
                    if (IsGridPos(x + direction.X, y + direction.Y) && _gridNodes[x + direction.X, y + direction.Y].Height - _gridNodes[x, y].Height == 1)
                    {
                        _gridNodes[x, y].AddConnection(_gridNodes[x + direction.X, y + direction.Y]);
                    }

                    direction.Rotate90DegreesClockwise();
                }
            }
        }
    }

    public int GetSumTrailHeadScores()
    {
        int sum = 0;

        foreach (GridNode gridNode in _gridNodes)
        {
            if (gridNode.Height == 0)
            {
                sum += gridNode.GetNumTrails(new());
            }
        }

        return sum;
    }

    public int GetSumTrailHeadRatings()
    {
        int sum = 0;

        foreach (GridNode gridNode in _gridNodes)
        {
            if (gridNode.Height == 0)
            {
                sum += gridNode.GetNumDistinctTrails();
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

    public void PrintConnections()
    {
        foreach (GridNode gridNode in _gridNodes)
        {
            gridNode.PrintConnections();
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

    public void Rotate90DegreesClockwise()
    {
        int oldX = X;
        X = Y;
        Y = -oldX;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}