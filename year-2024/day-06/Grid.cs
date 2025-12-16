namespace AdventOfCode.Year2024.Day06;

public class Grid
{
    private GridNode[,] _grid;
    private int Width => _grid.GetLength(1);
    private int Height => _grid.GetLength(0);

    public Grid(int width, int height)
    {
        _grid = new GridNode[width, height];
    }

    public void SetGridNode(int x, int y, char character)
    {
        _grid[x, y] = new(character == '#');
    }

    public int GetNumPatrolPositions(int x, int y)
    {
        int numVisited = 0;
        int currentX = x;
        int currentY = y;
        int[] currentDirection = { 0, 1 };

        while (IsGridPos(currentX, currentY))
        {
            if (!_grid[currentX, currentY].WasVisited)
            {
                _grid[currentX, currentY].Visit();
                numVisited++;
            }

            while (IsObstacle(currentX + currentDirection[0], currentY - currentDirection[1]))
            {
                RotateRight(currentDirection);
            }

            currentX += currentDirection[0];
            currentY -= currentDirection[1];
        }

        return numVisited;
    }

    public int GetNumObstaclePlacementLocations(int startX, int startY)
    {
        int num = 0;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (x == startX && y == startY)
                {
                    continue;
                }

                if (_grid[x, y].IsObstacle)
                {
                    continue;
                }

                _grid[x, y].SetAsObstacle(true);

                int currentX = startX;
                int currentY = startY;
                int[] currentDirection = { 0, 1 };

                while (IsGridPos(currentX, currentY))
                {
                    if (_grid[currentX, currentY].WasVisited && _grid[currentX, currentY].LastDirectionMoved[0] == currentDirection[0] && _grid[currentX, currentY].LastDirectionMoved[1] == currentDirection[1])
                    {
                        num++;
                        break;
                    }

                    _grid[currentX, currentY].Visit();
                    _grid[currentX, currentY].SetLastDirectionMoved(currentDirection[0], currentDirection[1]);

                    while (IsObstacle(currentX + currentDirection[0], currentY - currentDirection[1]))
                    {
                        RotateRight(currentDirection);
                    }

                    currentX += currentDirection[0];
                    currentY -= currentDirection[1];
                }

                _grid[x, y].SetAsObstacle(false);

                // Initailly my code didn't work in the for loops, but when I tried making individual nodes obstacles it gave the right answer.
                // Then I realized I needed to reset all the nodes, so nothing persisted in the next loop.
                // That took a solid hour and it is now 3am when I finished.
                foreach (GridNode gridNode in _grid)
                {
                    gridNode.Reset();
                }
            }
        }

        return num;
    }

    private bool IsGridPos(int x, int y)
    {
        return x >= 0 && x < _grid.GetLength(1) && y >= 0 && y < _grid.GetLength(0);
    }

    private bool IsObstacle(int x, int y)
    {
        return IsGridPos(x, y) && _grid[x, y].IsObstacle;
    }

    private void RotateRight(int[] direction)
    {
        int oldX = direction[0];
        direction[0] = direction[1];
        direction[1] = -oldX;
    }
}
