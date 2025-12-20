namespace AdventOfCode.Lib;

public class SolutionGenerator
{
    public static string GenerateSolutionTemplate(string year, string day)
    {
        return
        $$"""
        using AdventOfCode.Lib;

        namespace AdventOfCode.Year{{year}}.Day{{day}};

        [PuzzleName("")]
        public class Solution : Solver
        {
            public override object SolvePartOne(string[] input)
            {
                return "Not Solved Yet";
            }

            public override object SolvePartTwo(string[] input)
            {
                return "Not Solved Yet";
            }
        }
        
        """;
    }
}
