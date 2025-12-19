using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day04;

[PuzzleName("Camp Cleanup")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int count = 0;

        foreach (string line in input)
        {
            int index = line.IndexOf(",");
            if (Cleanup.FullyContain(line.Substring(0, index), line.Substring(index + 1)))
            {
                count++;
            }
        }

        return count;
    }

    public override object SolvePartTwo(string[] input)
    {
        int count2 = 0;

        foreach (string line in input)
        {
            int index = line.IndexOf(",");
            if (Cleanup.Overlap(line.Substring(0, index), line.Substring(index + 1)))
            {
                count2++;
            }
        }

        return count2;
    }
}