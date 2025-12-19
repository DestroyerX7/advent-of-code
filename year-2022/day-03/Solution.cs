using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day03;

[PuzzleName("Rucksack Reorganization")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int sum = 0;

        foreach (string line in input)
        {
            sum += Rucksack.Priorities(line);
        }

        return sum;
    }

    public override object SolvePartTwo(string[] input)
    {
        int sum2 = 0;

        for (int i = 0; i < input.Length; i += 3)
        {
            sum2 += Rucksack.PartTwo(input[i], input[i + 1], input[i + 2]);
        }

        return sum2;
    }
}