using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day01;

[PuzzleName("Calorie Counting")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        return Calories.MaxCalories(input, 1);
    }

    public override object SolvePartTwo(string[] input)
    {
        return Calories.MaxCalories(input, 3);
    }
}