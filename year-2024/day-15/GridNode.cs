namespace AdventOfCode.Year2024.Day15;

public class GridNode
{
    public ItemType ItemType { get; private set; }

    public GridNode(ItemType itemType)
    {
        ItemType = itemType;
    }
}

public enum ItemType
{
    Box,
    LeftBoxPart,
    RightBoxPart,
    Robot,
    Wall,
}