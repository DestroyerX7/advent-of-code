namespace AdventOfCode.Year2024.Day08;

public class GridNode
{
    public Vector2 Position { get; private set; }
    public char Character { get; private set; }
    public bool IsAntenna => Character != '.';
    public bool IsAntinode { get; private set; }

    public GridNode(Vector2 pos, char character)
    {
        Position = pos;
        Character = character;
    }

    public void SetIsAntinode(bool val)
    {
        IsAntinode = val;
    }
}