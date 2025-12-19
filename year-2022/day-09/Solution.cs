using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day09;

[PuzzleName("Rope Bridge")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Rope rope = new Rope();

        foreach (string line in input)
        {
            char moveDirection = line[0];
            int moveNum = int.Parse(line.Substring(2));
            rope.MoveHead(moveDirection, moveNum);
        }

        return rope.VisitedPositions.Count;
    }

    public override object SolvePartTwo(string[] input)
    {
        Rope rope = new Rope(10);

        foreach (string line in input)
        {
            char moveDirection = line[0];
            int moveNum = int.Parse(line.Substring(2));
            rope.MoveHead(moveDirection, moveNum);
        }

        return rope.VisitedPositions.Count;
    }
}