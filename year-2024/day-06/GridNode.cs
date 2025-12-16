namespace AdventOfCode.Year2024.Day06;

public class GridNode
{
    public bool IsObstacle { get; private set; }
    public bool WasVisited { get; private set; }
    public int[] LastDirectionMoved { get; private set; } = new int[2];

    public GridNode(bool isObstacle)
    {
        IsObstacle = isObstacle;
    }

    public void Visit()
    {
        WasVisited = true;
    }

    public void SetAsObstacle(bool value)
    {
        IsObstacle = value;
    }

    public void SetLastDirectionMoved(int xDirection, int yDirection)
    {
        LastDirectionMoved[0] = xDirection;
        LastDirectionMoved[1] = yDirection;
    }

    public void Reset()
    {
        WasVisited = false;
        LastDirectionMoved = new int[2];
    }
}