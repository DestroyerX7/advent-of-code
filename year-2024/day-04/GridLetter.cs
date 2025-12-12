namespace AdventOfCode.Year2024.Day04;

public class GridLetter
{
    private static readonly char[] _letters = { 'X', 'M', 'A', 'S' };

    private readonly int[] _pos = new int[2];
    public char Letter { get; private set; }
    private readonly Grid _grid;

    public GridLetter(int x, int y, char letter, Grid grid)
    {
        _pos[0] = x;
        _pos[1] = y;
        Letter = letter;
        _grid = grid;
    }

    public bool SearchPartOne(int letterIndex, int[] direction)
    {
        if (Letter == _letters[^1] && _letters[letterIndex] == Letter)
        {
            return true;
        }
        else if (_letters[letterIndex] == Letter)
        {
            return _grid.SearchPartOne(_pos[0] + direction[0], _pos[1] + direction[1], letterIndex + 1, direction);
        }

        return false;
    }
}