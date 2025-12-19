using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day08;

[PuzzleName("Treetop Tree House")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        List<List<int>> trees = new List<List<int>>();

        for (int i = 0; i < input.Length; i++)
        {
            trees.Add(new List<int>());

            for (int j = 0; j < input[i].Length; j++)
            {
                trees[i].Add(int.Parse(input[i][j].ToString()));
            }
        }

        int count = Treehouse.NumVisable(trees);
        return count;
    }

    public override object SolvePartTwo(string[] input)
    {
        List<List<int>> trees = new List<List<int>>();

        for (int i = 0; i < input.Length; i++)
        {
            trees.Add(new List<int>());

            for (int j = 0; j < input[i].Length; j++)
            {
                trees[i].Add(int.Parse(input[i][j].ToString()));
            }
        }

        return Treehouse.BestViewingDistance(trees);
    }
}