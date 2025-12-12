namespace AdventOfCode.Year2024.Day04;

public class Grid
{
    private static readonly int[,] _directions = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, -1 }, { -1, 1 } };

    private readonly GridLetter[,] _grid;
    private int Width => _grid.GetLength(1);
    private int Height => _grid.GetLength(0);

    public Grid(int width, int height)
    {
        _grid = new GridLetter[width, height];
    }

    public void AddGridLetter(int x, int y, char letter)
    {
        _grid[x, y] = new GridLetter(x, y, letter, this);
    }

    public int GetNumAppearancesPartOne()
    {
        int numAppearances = 0;

        foreach (GridLetter gridLetter in _grid)
        {
            if (gridLetter.Letter == 'X')
            {
                for (int i = 0; i < _directions.GetLength(0); i++)
                {
                    int[] direction = { _directions[i, 0], _directions[i, 1] };
                    bool found = gridLetter.SearchPartOne(0, direction);

                    if (found)
                    {
                        numAppearances++;
                    }
                }
            }
        }

        return numAppearances;
    }

    // Kind of a brute force solution, but it works
    public int GetNumAppearancesPartTwo()
    {
        int numAppearances = 0;

        for (int y = 1; y < _grid.GetLength(0) - 1; y++)
        {
            for (int x = 1; x < _grid.GetLength(1) - 1; x++)
            {
                if (_grid[x, y].Letter == 'A')
                {
                    if (_grid[x - 1, y + 1].Letter == 'M' && _grid[x + 1, y + 1].Letter == 'M' && _grid[x - 1, y - 1].Letter == 'S' && _grid[x + 1, y - 1].Letter == 'S')
                    {
                        numAppearances++;
                    }
                    else if (_grid[x + 1, y + 1].Letter == 'M' && _grid[x + 1, y - 1].Letter == 'M' && _grid[x - 1, y - 1].Letter == 'S' && _grid[x - 1, y + 1].Letter == 'S')
                    {
                        numAppearances++;
                    }
                    else if (_grid[x - 1, y - 1].Letter == 'M' && _grid[x + 1, y - 1].Letter == 'M' && _grid[x - 1, y + 1].Letter == 'S' && _grid[x + 1, y + 1].Letter == 'S')
                    {
                        numAppearances++;
                    }
                    else if (_grid[x - 1, y + 1].Letter == 'M' && _grid[x - 1, y - 1].Letter == 'M' && _grid[x + 1, y - 1].Letter == 'S' && _grid[x + 1, y + 1].Letter == 'S')
                    {
                        numAppearances++;
                    }
                }
            }
        }

        return numAppearances;
    }

    public bool SearchPartOne(int x, int y, int letterIndex, int[] direction)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return false;
        }

        return _grid[x, y].SearchPartOne(letterIndex, direction);
    }
}