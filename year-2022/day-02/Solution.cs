using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day02;

[PuzzleName("Rock Paper Scissors")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int sum = 0;

        foreach (string line in input)
        {
            char[] chars = line.ToCharArray();

            sum += Strategy.ScorePartOne(chars[0], chars[2]);
        }

        return sum;
    }

    public override object SolvePartTwo(string[] input)
    {
        int sum = 0;

        foreach (string line in input)
        {
            char[] chars = line.ToCharArray();

            sum += Strategy.ScorePartTwo(chars[0], chars[2]);
        }

        return sum;
    }
}