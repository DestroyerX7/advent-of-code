using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day06;

[PuzzleName("Tuning Trouble")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        return Datastream.StartOfPacket(input[0]);
    }

    public override object SolvePartTwo(string[] input)
    {
        return Datastream.StartOfMessage(input[0]);
    }
}